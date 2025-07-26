using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniProject.Api.Models;
using MiniProject.Api.Services;
using MiniProject.Api.Attributes;

namespace MiniProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [RateLimit(seconds: 60, maxRequests: 5)]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var isValid = _authService.ValidateUser(user.Username, user.Password);
            if (!isValid)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _authService.GenerateToken(user.Username);
            return Ok(new { Token = token });
        }
    }
}