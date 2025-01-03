using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _studentRepository;

        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }

        [HttpPost("convert-time")]
        public async Task<ActionResult<StudentResponse>> ConvertStudentTimeAsync([FromBody] StudentRequest studentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var studentResponse = await _studentRepository.ConvertStudentTimeAsync(studentRequest);
                if (studentResponse == null)
                {
                    return StatusCode(500, "Failed to convert time.");
                }

                return Ok(studentResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Request error occurred while converting time.");
                return StatusCode(503, "Service unavailable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
