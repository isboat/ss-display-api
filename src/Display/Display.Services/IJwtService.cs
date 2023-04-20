using System.IdentityModel.Tokens.Jwt;

namespace Display.Services
{
    public interface IJwtService
    {
        string GenerateToken(IDictionary<string, string> tokenData, DateTime? expiresOn);

        void ValidateToken(string token);

        JwtSecurityToken ReadToken(string token);
    }
}
