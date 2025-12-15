use axum::{
    routing::{get, post, put, delete},
    Router,
};
use std::net::SocketAddr;
use std::sync::Arc;
use tower_http::cors::{CorsLayer, AllowOrigin, AllowMethods, AllowHeaders};
use http::{header, HeaderValue, Method};

mod auth;
mod config;
mod db;
mod handlers;
mod models;

use config::Config;
use db::DB;
use handlers::{
    delete_user_handler, get_me_handler, get_users_handler, health_checker_handler,
    login_user_handler, register_user_handler, update_user_handler,
};

pub struct AppState {
    db: DB,
    env: Config,
}

#[tokio::main]
async fn main() {
    // Initialize tracing
    tracing_subscriber::fmt::init();

    let config = Config::init();
    let pool = db::connection_pool(&config.database_url).await.unwrap();

    let app_state = Arc::new(AppState {
        db: pool.clone(),
        env: config.clone(),
    });

    let cors = CorsLayer::new()
        .allow_origin("http://localhost:5173".parse::<HeaderValue>().unwrap())
        .allow_methods([Method::GET, Method::POST, Method::PUT, Method::DELETE, Method::OPTIONS])
        .allow_headers([header::CONTENT_TYPE, header::AUTHORIZATION, header::ACCEPT])
        .allow_credentials(true);

    let app = Router::new()
        .route("/api/healthchecker", get(health_checker_handler))
        .route("/api/auth/register", post(register_user_handler))
        .route("/api/auth/login", post(login_user_handler))
        .route("/api/users/me", get(get_me_handler))
        .route("/api/users", get(get_users_handler))
        .route("/api/users/:id", put(update_user_handler).delete(delete_user_handler))
        .layer(cors)
        .with_state(app_state);

    let addr = SocketAddr::from(([127, 0, 0, 1], 8080));
    println!("listening on {}", addr);
    let listener = tokio::net::TcpListener::bind(addr).await.unwrap();
    axum::serve(listener, app).await.unwrap();
}
