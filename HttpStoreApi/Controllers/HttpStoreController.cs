using Microsoft.AspNetCore.Mvc;
using HttpStoreApi.Models;
using HttpStoreApi.Repositories;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HttpStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpStoreController : ControllerBase
    {
        private readonly IHttpStoreRepository _httpStoreRepository;

        public HttpStoreController(IHttpStoreRepository IHttpStoreRepository)
        {
            _httpStoreRepository = IHttpStoreRepository;
        }

        [HttpPost("call-api")]
        public async Task<IActionResult> CallApi([FromBody] UnifiedApiRequest request)
        {
            if (string.IsNullOrEmpty(request.ApiId))
            {
                return BadRequest("ApiId is required.");
            }

            string result;
            switch (request.ApiId)
            {
                case "1":
                    var timeZoneRequest = JsonConvert.DeserializeObject<TimeZoneRequest>(request.Request.ToString());
                    result = await _httpStoreRepository.CallTimeZoneApiAsync(timeZoneRequest);
                    break;
                case "2":
                    try
                    {
                        var packetRequest = JsonConvert.DeserializeObject<PacketRequest>(request.Request.ToString());
                        result = await _httpStoreRepository.CallPacketApiAsync(packetRequest);
                    }
                    catch (JsonException ex)
                    {
                        return BadRequest($"Error deserializing request: {ex.Message}");
                    }
                    break;
                default:
                    return BadRequest("Invalid API id.");
            }

            var jsonResult = JsonConvert.DeserializeObject(result);
            return Ok(jsonResult);
        }
    }
}
