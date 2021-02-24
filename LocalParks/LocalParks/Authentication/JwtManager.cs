using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LocalParks.Authentication
{
    public static class JwtManager
    {
        private const string Secret = "TG9jYWxQYXJrc0FQSV9LZXkxX093ZW5TdGVlbGU=";

        public static string GenerateToken(string username, int expireMinutes = 60000)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            // expected: "LocalParksAPI_Key1_OwenSteele"
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, username)
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
