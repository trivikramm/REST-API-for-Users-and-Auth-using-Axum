# Mini Project Workspace

This repository contains a mini project that includes a .NET backend and an Angular frontend. The project features a login page and a dashboard.

## Project Structure

```
mini-project-workspace
├── backend
│   ├── MiniProject.Api
│   │   ├── Controllers
│   │   ├── Models
│   │   ├── Services
│   │   ├── Program.cs
│   │   ├── Startup.cs
│   │   └── MiniProject.Api.csproj
│   └── README.md
├── frontend
│   ├── src
│   │   ├── app
│   │   │   ├── login
│   │   │   ├── dashboard
│   │   │   └── app.module.ts
│   │   └── main.ts
│   ├── angular.json
│   ├── package.json
│   └── README.md
└── README.md
```

## Backend

The backend is built using .NET and provides an API for user authentication. It includes:

- **AuthController**: Handles user authentication requests.
- **User Model**: Represents the user entity.
- **AuthService**: Contains business logic for user validation and token generation.
- **Startup.cs**: Configures services and middleware for the application.

### Setup Instructions

1. Navigate to the `backend` directory.
2. Restore the dependencies using the command:
   ```
   dotnet restore
   ```
3. Run the application using:
   ```
   dotnet run
   ```

## Frontend

The frontend is built using Angular and provides a user interface for the application. It includes:

- **Login Component**: Manages user login functionality.
- **Dashboard Component**: Displays user-specific data after login.
- **App Module**: Main module that imports necessary Angular modules.

### Setup Instructions

1. Navigate to the `frontend` directory.
2. Install the dependencies using the command:
   ```
   npm install
   ```
3. Start the Angular application using:
   ```
   ng serve
   ```

## Conclusion

This mini project serves as a foundational example of integrating a .NET backend with an Angular frontend, focusing on user authentication and data display.