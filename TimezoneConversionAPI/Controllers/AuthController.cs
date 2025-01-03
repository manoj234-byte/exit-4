using Microsoft.AspNetCore.Mvc;
using System;

namespace TimezoneConversionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("generate-token")]
        public IActionResult GenerateToken()
        {
            // Simple Bearer token for example purposes
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return Ok(new { token });
        }
    }
}
