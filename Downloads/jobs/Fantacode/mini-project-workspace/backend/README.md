# Mini Project Backend API

This is the backend API for the mini project, which includes user authentication and a basic structure for handling requests.

## Getting Started

### Prerequisites

- .NET SDK (version 6.0 or later)
- A code editor (e.g., Visual Studio Code)
- Postman or any API testing tool

### Installation

1. Clone the repository:
   ```
   git clone <repository-url>
   ```

2. Navigate to the backend directory:
   ```
   cd mini-project-workspace/backend/MiniProject.Api
   ```

3. Restore the dependencies:
   ```
   dotnet restore
   ```

### Running the Application

To run the application, use the following command:
```
dotnet run
```

The API will start on `http://localhost:5000` by default.

### API Endpoints

- **POST /api/auth/login**: Authenticates a user and returns a token.
  - Request Body:
    ```json
    {
      "username": "string",
      "password": "string"
    }
    ```

### Testing

You can use Postman or any other API testing tool to test the endpoints. Make sure to set the appropriate headers and request body as specified.

### Contributing

Feel free to submit issues or pull requests for improvements or bug fixes.

### License

This project is licensed under the MIT License.