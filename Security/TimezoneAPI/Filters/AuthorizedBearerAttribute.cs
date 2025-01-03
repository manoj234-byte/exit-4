using Microsoft.AspNetCore.Authorization;

public class AuthorizedBearerAttribute : Attribute, IAuthorizationRequirement
{
    public string AllowedId { get; set; }

    public AuthorizedBearerAttribute(string allowedId)
    {
        AllowedId = allowedId;
    }
}
