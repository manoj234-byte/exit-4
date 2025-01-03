using Microsoft.AspNetCore.Mvc;
using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationApi.Controllers
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
        public IActionResult Login([FromBody] User user)
        {
            if (!_authService.ValidateUser(user.Username, user.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _authService.GenerateJwtToken(user.Username);
            return Ok(new AuthResponse
            {
                Token = token,
                Username = user.Username
            });
        }

        [HttpGet("profile")]
        [Authorize]
        public IActionResult Profile()
        {
            var username = User.Identity?.Name;
            return Ok(new { Message = $"Hello, {username}. This is your profile." });
        }
    }
}
