using System.Security.Claims;

namespace Mangos.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string type, string defaultValue = "")
        {
            var claim = principal.FindFirst(type);

            if (claim == null)
                return defaultValue;

            return claim.Value;
        }
    }
}