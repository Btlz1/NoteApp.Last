using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using btlz.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace btlz.Settings;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator
{
    public string GenerateToken(User user)
    {
        var options = jwtSettings.Value;
        
        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
            SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Login),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        var expiration = DateTime.UtcNow.AddMinutes(5);

        JwtSecurityToken securityToken = new(
            issuer: options.Issuer, 
            audience: options.Audience, 
            expires: expiration,
            claims: claims,
            signingCredentials: signingCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }
}