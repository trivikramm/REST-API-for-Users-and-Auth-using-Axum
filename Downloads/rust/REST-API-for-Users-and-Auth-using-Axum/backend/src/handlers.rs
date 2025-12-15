use axum::{
    extract::{Path, State},
    http::{header, StatusCode},
    response::{IntoResponse, Json},
    Extension,
};
use axum_extra::{
    headers::{authorization::Bearer, Authorization},
    TypedHeader,
};
use serde_json::json;
use std::sync::Arc;
use uuid::Uuid;

use crate::{
    auth::{create_jwt, hash_password, verify_password, verify_token},
    db::DB,
    models::{CreateUserSchema, LoginUserSchema, User, UserResponse},
    AppState,
};

// Middleware-like extractor helper
async fn get_authenticated_user(
    State(data): State<Arc<AppState>>,
    TypedHeader(Authorization(bearer)): TypedHeader<Authorization<Bearer>>,
) -> Result<User, (StatusCode, Json<serde_json::Value>)> {
    let token = bearer.token();
    let claims = verify_token(token, &data.env.jwt_secret).map_err(|_| {
        (
            StatusCode::UNAUTHORIZED,
            Json(json!({"error": "Invalid token"})),
        )
    })?;

    let user_id = Uuid::parse_str(&claims.sub).map_err(|_| {
        (
            StatusCode::UNAUTHORIZED,
            Json(json!({"error": "Invalid token claims"})),
        )
    })?;

    let user = sqlx::query_as!(User, "SELECT * FROM users WHERE id = $1", user_id)
        .fetch_optional(&data.db)
        .await
        .map_err(|e| {
            (
                StatusCode::INTERNAL_SERVER_ERROR,
                Json(json!({"error": "Database error"})),
            )
        })?
        .ok_or((
            StatusCode::UNAUTHORIZED,
            Json(json!({"error": "User not found"})),
        ))?;

    Ok(user)
}

pub async fn health_checker_handler() -> impl IntoResponse {
    const MESSAGE: &str = "Build Simple CRUD API with Rust and Axum";
    let json_response = serde_json::json!({
        "status": "success",
        "message": MESSAGE
    });
    Json(json_response)
}

pub async fn register_user_handler(
    State(data): State<Arc<AppState>>,
    Json(body): Json<CreateUserSchema>,
) -> Result<impl IntoResponse, (StatusCode, Json<serde_json::Value>)> {
    let user_exists = sqlx::query_scalar!(
        "SELECT EXISTS(SELECT 1 FROM users WHERE email = $1)",
        body.email
    )
    .fetch_one(&data.db)
    .await
    .map_err(|e| {
        (
            StatusCode::INTERNAL_SERVER_ERROR,
            Json(json!({"status": "error", "message": "Database error"})),
        )
    })?;

    if let Some(exists) = user_exists {
        if exists {
            return Err((
                StatusCode::CONFLICT,
                Json(json!({"status": "fail", "message": "User with that email already exists"})),
            ));
        }
    }

    let hashed_password = hash_password(&body.password).map_err(|e| {
        (
            StatusCode::INTERNAL_SERVER_ERROR,
            Json(json!({"status": "error", "message": "Password hashing failed"})),
        )
    })?;

    let user = sqlx::query_as!(
        User,
        "INSERT INTO users (name, email, password) VALUES ($1, $2, $3) RETURNING *",
        body.name,
        body.email,
        hashed_password
    )
    .fetch_one(&data.db)
    .await
    .map_err(|e| {
        (
            StatusCode::INTERNAL_SERVER_ERROR,
            Json(json!({"status": "error", "message": "Failed to create user"})),
        )
    })?;

    let user_response = serde_json::json!({
        "status": "success",
        "data": {
            "user": filter_user_record(&user)
        }
    });

    Ok(Json(user_response))
}

pub async fn login_user_handler(
    State(data): State<Arc<AppState>>,
    Json(body): Json<LoginUserSchema>,
) -> Result<impl IntoResponse, (StatusCode, Json<serde_json::Value>)> {
    let user = sqlx::query_as!(User, "SELECT * FROM users WHERE email = $1", body.email)
        .fetch_optional(&data.db)
        .await
        .map_err(|e| {
            (
                StatusCode::INTERNAL_SERVER_ERROR,
                Json(json!({"status": "error", "message": "Database error"})),
            )
        })?
        .ok_or((
            StatusCode::BAD_REQUEST,
            Json(json!({"status": "fail", "message": "Invalid email or password"})),
        ))?;

    let is_valid = verify_password(&body.password, &user.password).unwrap_or(false);

    if !is_valid {
        return Err((
            StatusCode::BAD_REQUEST,
            Json(json!({"status": "fail", "message": "Invalid email or password"})),
        ));
    }

    let token = create_jwt(&user.id.to_string(), &data.env.jwt_secret).map_err(|e| {
        (
            StatusCode::INTERNAL_SERVER_ERROR,
            Json(json!({"status": "error", "message": "Token creation failed"})),
        )
    })?;

    let cookie = format!("token={}; HttpOnly; Path=/; Max-Age=3600;", token);

    let mut response = Json(json!({
        "status": "success",
        "token": token
    }))
    .into_response();

    response
        .headers_mut()
        .insert(header::SET_COOKIE, cookie.parse().unwrap());

    Ok(response)
}

pub async fn get_me_handler(
    State(data): State<Arc<AppState>>,
    TypedHeader(Authorization(bearer)): TypedHeader<Authorization<Bearer>>,
) -> Result<impl IntoResponse, (StatusCode, Json<serde_json::Value>)> {
    let user = get_authenticated_user(State(data), TypedHeader(Authorization(bearer))).await?;

    Ok(Json(json!({
        "status": "success",
        "data": {
            "user": filter_user_record(&user)
        }
    })))
}

pub async fn get_users_handler(
    State(data): State<Arc<AppState>>,
    TypedHeader(Authorization(bearer)): TypedHeader<Authorization<Bearer>>,
) -> Result<impl IntoResponse, (StatusCode, Json<serde_json::Value>)> {
    let _user = get_authenticated_user(State(data.clone()), TypedHeader(Authorization(bearer))).await?;

    let users = sqlx::query_as!(User, "SELECT * FROM users")
        .fetch_all(&data.db)
        .await
        .map_err(|e| {
            (
                StatusCode::INTERNAL_SERVER_ERROR,
                Json(json!({"status": "error", "message": "Database error"})),
            )
        })?;

    let user_responses: Vec<UserResponse> = users.iter().map(|user| filter_user_record(user)).collect();

    Ok(Json(json!({
        "status": "success",
        "results": user_responses.len(),
        "data": {
            "users": user_responses
        }
    })))
}

fn filter_user_record(user: &User) -> UserResponse {
    UserResponse {
        id: user.id,
        name: user.name.to_owned(),
        email: user.email.to_owned(),
        role: user.role.clone(),
        created_at: user.created_at,
        updated_at: user.updated_at,
    }
}

pub async fn update_user_handler(
    Path(id): Path<Uuid>,
    State(data): State<Arc<AppState>>,
    TypedHeader(Authorization(bearer)): TypedHeader<Authorization<Bearer>>,
    Json(body): Json<serde_json::Value>, // Using Value for partial updates or define a specific schema
) -> Result<impl IntoResponse, (StatusCode, Json<serde_json::Value>)> {
    let _user = get_authenticated_user(State(data.clone()), TypedHeader(Authorization(bearer))).await?;

    let user = sqlx::query_as!(User, "SELECT * FROM users WHERE id = $1", id)
        .fetch_optional(&data.db)
        .await
        .map_err(|e| {
            (
                StatusCode::INTERNAL_SERVER_ERROR,
                Json(json!({"status": "error", "message": "Database error"})),
            )
        })?
        .ok_or((
            StatusCode::NOT_FOUND,
            Json(json!({"status": "fail", "message": "User not found"})),
        ))?;

    // Simple update logic (only name for now as example)
    let name = body.get("name").and_then(|v| v.as_str()).unwrap_or(&user.name);
    
    let updated_user = sqlx::query_as!(
        User,
        "UPDATE users SET name = $1, updated_at = NOW() WHERE id = $2 RETURNING *",
        name,
        id
    )
    .fetch_one(&data.db)
    .await
    .map_err(|e| {
        (
            StatusCode::INTERNAL_SERVER_ERROR,
            Json(json!({"status": "error", "message": "Failed to update user"})),
        )
    })?;

    Ok(Json(json!({
        "status": "success",
        "data": {
            "user": filter_user_record(&updated_user)
        }
    })))
}

pub async fn delete_user_handler(
    Path(id): Path<Uuid>,
    State(data): State<Arc<AppState>>,
    TypedHeader(Authorization(bearer)): TypedHeader<Authorization<Bearer>>,
) -> Result<impl IntoResponse, (StatusCode, Json<serde_json::Value>)> {
    let _user = get_authenticated_user(State(data.clone()), TypedHeader(Authorization(bearer))).await?;

    let rows_affected = sqlx::query!("DELETE FROM users WHERE id = $1", id)
        .execute(&data.db)
        .await
        .map_err(|e| {
            (
                StatusCode::INTERNAL_SERVER_ERROR,
                Json(json!({"status": "error", "message": "Database error"})),
            )
        })?
        .rows_affected();

    if rows_affected == 0 {
        return Err((
            StatusCode::NOT_FOUND,
            Json(json!({"status": "fail", "message": "User not found"})),
        ));
    }

    Ok(Json(json!({
        "status": "success",
        "message": "User deleted successfully"
    })))
}
