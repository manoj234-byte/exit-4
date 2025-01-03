using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using StudentApi.Models;
using TimePacketApi.Models; // Make sure this is the correct namespace for your models

public class StudentRepository : IStudentRepository
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentRepository> _logger;
    private readonly string _baseUrl;

    public StudentRepository(HttpClient httpClient, ILogger<StudentRepository> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["TimePacketApiUrl"] ?? "http://localhost:5230/TimeZone"; // Default URL
    }

    public async Task<StudentResponse?> ConvertStudentTimeAsync(StudentRequest studentRequest)
    {
        var timeZoneRequest = new PacketRequest
        {
            PacketId = Guid.NewGuid().ToString(),
            SagaId = Guid.NewGuid().ToString(),
            Request = new RequestDetails
            {
                Id = Guid.NewGuid().ToString(),
                Name = studentRequest.StudentName ?? "Default Name",
                MaId = "your-ma-id", // Replace with actual MaId if needed
                DateTime = studentRequest.DateTime.ToString("o"),
                Payload = new TimeConversionPayload
                {
                    DateTime = studentRequest.DateTime,
                    TargetTimeZone = studentRequest.TargetTimeZone
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(_baseUrl, timeZoneRequest);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error from TimePacket API: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
            return null;
        }

        var timeZoneResponse = await response.Content.ReadFromJsonAsync<PacketResponse>();

        if (timeZoneResponse == null || timeZoneResponse.Response == null || timeZoneResponse.Response.Payload == null)
        {
            _logger.LogError("Error parsing PacketResponse or Payload is null.");
            return null;
        }

        // Access the ConvertedTime property directly
        var payload = timeZoneResponse.Response.Payload;

        return new StudentResponse
        {
            StudentName = studentRequest.StudentName,
            ConvertedTime = payload.ConvertedTime, // Access the property correctly
            TargetTimeZone = studentRequest.TargetTimeZone
        };
    }
}
