using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Your JWT token validation logic here
        options.Authority = "https://your-identity-server";
        options.Audience = "your-api";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BearerPolicy", policy =>
        policy.Requirements.Add(new AuthorizedBearerAttribute("124")));
});

builder.Services.AddSingleton<IAuthorizationHandler, AuthorizedBearerHandler>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Http Store API v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
