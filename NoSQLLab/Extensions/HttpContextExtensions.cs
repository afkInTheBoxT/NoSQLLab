using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NoSQLLab.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetNameFromToken(this HttpContext
            context)
        {
            Claim userClaim =
                context?.User.Claims.FirstOrDefault(c => c.Type ==
                                                         ClaimTypes.Name);
            return userClaim?.Value;
        }
    }

}