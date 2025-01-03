using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using System;

namespace TimezoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimezoneController : ControllerBase
    {
        // POST api/timezone/convert
        [HttpPost("convert")]
        [Authorize(Policy = "BearerPolicy")]
        public IActionResult ConvertToIST([FromBody] TimezoneRequest request)
        {
            try
            {
                // Convert input datetime to IST
                var istZone = DateTimeZoneProviders.Tzdb["Asia/Kolkata"];
                var localDateTime = LocalDateTime.FromDateTime(request.DateTime);
                var instant = istZone.AtLeniently(localDateTime).ToInstant();

                var response = new TimezoneResponse
                {
                    LocalDateTime = instant.ToDateTimeUtc(),
                    Timezone = "Asia/Kolkata (IST)"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error converting time: {ex.Message}");
            }
        }
    }
}
