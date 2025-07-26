using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MiniProject.Api.Models;

namespace MiniProject.Api.Services
{
    public class AuthService
    {
        private readonly List<User> _users = new List<User>
        {
            new User { Id = 1, Username = "testuser", Password = "password" }
        };

        public bool ValidateUser(string username, string password)
        {
            var user = _users.Find(u => u.Username == username && u.Password == password);
            return user != null;
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_super_secret_key_123_this_is_a_very_long_key_for_jwt_authentication!@#$%^&*()"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}