using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using NoSQLLab.Auth;

namespace NoSQLLab
{
    public class JWTTokenizer
    {
        public static string GetEncodedJWT(ClaimsIdentity
            identity, TimeSpan lifetime)
        {
            DateTime now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(lifetime),
                signingCredentials: new
                    SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));
            return new
                JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}