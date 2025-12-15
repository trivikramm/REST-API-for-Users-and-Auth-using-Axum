# Rust Full-Stack Application

A REST API built with **Rust + Axum** backend and **React + TypeScript + Vite** frontend.

## Prerequisites

- [Rust](https://www.rust-lang.org/tools/install) (latest stable)
- [Node.js](https://nodejs.org/) (v18+)
- [PostgreSQL](https://www.postgresql.org/) database
- [Docker](https://www.docker.com/) (optional, for containerized setup)

## Backend Setup

```bash
cd backend

# Create .env file with required variables
# DATABASE_URL=postgresql://user:password@localhost:5432/dbname
# JWT_SECRET=your-secret-key

# Run database migrations (requires sqlx-cli)
sqlx database create
sqlx migrate run

# Run the backend server
cargo run
```

The backend will start at `http://127.0.0.1:8080`

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/healthchecker` | Health check |
| POST | `/api/auth/register` | Register user |
| POST | `/api/auth/login` | Login user |
| GET | `/api/users/me` | Get current user |
| GET | `/api/users` | List all users |
| PUT | `/api/users/:id` | Update user |
| DELETE | `/api/users/:id` | Delete user |

## Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Run development server
npm run dev
```

The frontend will start at `http://localhost:5173`

## Docker Setup

```bash
# Run both services with Docker Compose
docker-compose up
```

## Tech Stack

**Backend:**
- Axum (web framework)
- SQLx (PostgreSQL)
- JWT authentication
- Argon2 password hashing

**Frontend:**
- React 19
- TypeScript
- Vite
