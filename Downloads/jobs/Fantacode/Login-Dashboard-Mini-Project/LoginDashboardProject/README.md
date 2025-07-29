# Login Dashboard Mini Project

Welcome! This is a simple, modern full-stack project to help you learn and demo a login system and dashboard using a .NET backend and Angular frontend.

---

## Project Structure

```
Login-Dashboard-Mini-Project/
└── LoginDashboardProject/
    ├── backend/    # .NET 8 Web API
    ├── frontend/   # Angular app
    └── README.md
```

---

## Features

- User login with JWT authentication
- Dashboard with chart data
- Rate limiting middleware for security
- Modern Angular UI

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js (v16+ recommended)](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)

### 1. Clone the Repository

```bash
git clone <repo-url>
cd Login-Dashboard-Mini-Project/LoginDashboardProject
```

### 2. Install Dependencies

Backend:
```bash
cd backend
dotnet restore
```

Frontend:
```bash
cd ../frontend
npm install
```

### 3. Run the App

**Backend:**
```bash
cd backend
dotnet run
```

**Frontend:**
```bash
cd frontend
npx ng serve --host 0.0.0.0 --disable-host-check
```

Or use the VS Code tasks: `Run .NET Backend` and `Run Angular Frontend`.

---

## Access

- Frontend: [http://localhost:4200](http://localhost:4200)
- Backend API: [http://localhost:5000](http://localhost:5000) (default)

---

## Configuration & Tips

- Backend config: `backend/appsettings.json`, `appsettings.Development.json`
- Frontend API proxy: `frontend/proxy.conf.json`
- If you see OpenSSL errors, the frontend uses `--openssl-legacy-provider` for compatibility.
- Make sure ports 4200 (frontend) and 5000 (backend) are open.

---

## Contributing

Pull requests and suggestions are welcome! If you spot a bug or want to add a feature, just open an issue or PR.

---