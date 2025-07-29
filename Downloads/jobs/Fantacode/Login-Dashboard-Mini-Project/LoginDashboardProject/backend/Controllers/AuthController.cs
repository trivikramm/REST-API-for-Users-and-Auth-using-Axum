using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using LoginDashboardAPI.Models;
using LoginDashboardAPI.Services;

namespace LoginDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var loginResponse = await _authService.AuthenticateAsync(loginRequest);

            if (loginResponse == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(loginResponse);
        }
    }
}