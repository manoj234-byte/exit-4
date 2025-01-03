using Microsoft.AspNetCore.Mvc;
using TimezoneConversionAPI.Models;
using TimezoneConversionAPI.Services;

namespace TimezoneConversionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [BearerAuthorize]
    public class TimezoneController : ControllerBase
    {
        private readonly TimezoneService _timezoneService;

        public TimezoneController(TimezoneService timezoneService)
        {
            _timezoneService = timezoneService;
        }

        [HttpPost("convert")]
        public IActionResult ConvertTimeZone([FromBody] TimezoneConversionRequest request)
        {
            if (string.IsNullOrEmpty(request.TargetTimeZone))
            {
                return BadRequest("Target time zone is required.");
            }

            try
            {
                var response = _timezoneService.ConvertTimeZone(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting time: {ex.Message}");
            }
        }
    }
}
