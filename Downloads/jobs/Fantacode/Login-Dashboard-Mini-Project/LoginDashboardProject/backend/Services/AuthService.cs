using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LoginDashboardAPI.Models;
using LoginDashboardAPI.Services;

namespace LoginDashboardAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly List<User> _users; // This should ideally come from a database
        private readonly string _secretKey;

        public AuthService(string secretKey)
        {
            _secretKey = secretKey;
            _users = new List<User>
            {
                new User { Username = "testuser", Password = "password" }, // Example user
                new User { Username = "admin", Password = "password" } // Adding additional user for testing
            };
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            Console.WriteLine($"Attempting to authenticate user: {request.Username}");
            var user = _users.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user == null)
            {
                Console.WriteLine($"Authentication failed for user: {request.Username}");
                return null!; // Returning null with null-forgiving operator
            }

            Console.WriteLine($"Authentication successful for user: {request.Username}");
            var token = await GenerateToken(user);
            return new LoginResponse { Token = token, User = user };
        }

        public async Task<string> GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            Console.WriteLine($"Secret key length: {_secretKey.Length}");
            Console.WriteLine($"Using secret key: {_secretKey}");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($"Generated token: {tokenString.Substring(0, 20)}...");
            
            return await Task.FromResult(tokenString);
        }
    }
}