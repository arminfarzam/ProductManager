using System.Security.Claims;

namespace ProductManager.Application.Utilities.Extensions.Identity;

public static class IdentityUserExtension
{
    public static string GetIdentityUsername(this ClaimsPrincipal claimsPrincipal)
    {
        var result = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
        return result ?? throw new Exception("UnAuthorized User");
    }
}