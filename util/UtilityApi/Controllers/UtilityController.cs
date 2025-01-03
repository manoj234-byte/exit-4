using Microsoft.AspNetCore.Mvc;

namespace UtilityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase
    {
        [HttpGet("reverse-string")]
        public IActionResult ReverseString([FromQuery] string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return BadRequest("Input string cannot be null or empty.");
            }

            var reversed = new string(input.Reverse().ToArray());
            return Ok(new { Original = input, Reversed = reversed });
        }

        [HttpGet("days-between")]
        public IActionResult DaysBetween([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (endDate < startDate)
            {
                return BadRequest("End date must be greater than or equal to start date.");
            }

            var daysBetween = (endDate - startDate).Days;
            return Ok(new { StartDate = startDate, EndDate = endDate, DaysBetween = daysBetween });
        }
    }
}
