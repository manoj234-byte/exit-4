using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TimezoneConversionAPI.Services;
using TimezoneConversionAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<TimezoneService>();

builder.Services.AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("Bearer", null);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>
        policy.RequireClaim("juid"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middleware for logging requests and responses
app.Use(async (context, next) =>
{
    // Log the request
    var request = context.Request;
    System.Console.WriteLine($"Request: {request.Method} {request.Path}");

    // Call the next delegate/middleware in the pipeline
    await next.Invoke();

    // Log the response
    var response = context.Response;
    System.Console.WriteLine($"Response: {response.StatusCode}");
});

// Middleware for exception handling
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"Exception: {ex.Message}");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

// Use Swagger for API documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timezone Conversion API v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Custom Bearer Authentication Handler
public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string PredefinedToken = "abc123"; // Predefined Bearer token

    public BearerAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        string token = Request.Headers["Authorization"].ToString().Split(" ").Last();

        if (token != PredefinedToken)
        {
            return AuthenticateResult.Fail("Invalid Bearer Token");
        }

        var claims = new[] { new Claim("juid", "example-juid") };
        var identity = new ClaimsIdentity(claims, "Bearer");
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

// Custom BearerAuthorize attribute
public class BearerAuthorizeAttribute : AuthorizeAttribute
{
    public BearerAuthorizeAttribute()
    {
        Policy = "Bearer";
    }
}
