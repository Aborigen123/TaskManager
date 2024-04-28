using System.Security.Claims;

namespace Service.ExtensionMethods
{
    public static class ClaimExtensions
    {
        public static string GetUserName(this IEnumerable<Claim> claims)
        {
            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }
    }
}
