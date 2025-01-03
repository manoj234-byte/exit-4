using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class AuthorizedBearerHandler : AuthorizationHandler<AuthorizedBearerAttribute>
{
    private readonly ILogger<AuthorizedBearerHandler> _logger;

    public AuthorizedBearerHandler(ILogger<AuthorizedBearerHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizedBearerAttribute requirement)
    {
        // Extract the 'id' from the user's claims
        var tokenId = context.User?.FindFirst(c => c.Type == "id")?.Value;

        if (tokenId == requirement.AllowedId)
        {
            context.Succeed(requirement); // Authorization successful
        }
        else
        {
            _logger.LogWarning($"Unauthorized access attempt with ID: {tokenId}");
        }

        return Task.CompletedTask;
    }
}
