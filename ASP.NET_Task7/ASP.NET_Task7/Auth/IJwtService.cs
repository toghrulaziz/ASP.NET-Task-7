using System.Security.Claims;

namespace ASP.NET_Task7.Auth
{
    public interface IJwtService
    {
        string GenerateSecurityToken(string id, string email, IEnumerable<string> roles, IEnumerable<Claim> userClaims);
    }
}
