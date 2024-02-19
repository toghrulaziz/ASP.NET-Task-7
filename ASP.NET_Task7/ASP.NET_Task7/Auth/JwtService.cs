using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_Task7.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _config;

        public JwtService(JwtConfig config)
        {
            _config = config;
        }

        public string GenerateSecurityToken(string id, string email, IEnumerable<string> roles, IEnumerable<Claim> userClaims)
        {
            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim("userId", id),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, string.Join(",", roles))
            }.Concat(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                expires: DateTime.Now.AddMinutes(_config.ExpireMinutes),
                signingCredentials: signingCredentials,
                claims: claims
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return accessToken;
        }
    }
}
