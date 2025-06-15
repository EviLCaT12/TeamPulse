using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TeamPulse.Accounts.Application;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Infrastructure.Options;

namespace TeamPulse.Accounts.Infrastructure.Jwt;

public class JwtTokenProvider : ITokenProvider 
{
    private readonly JwtOptions _options;

    public JwtTokenProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    public string GenerateTokenAsync(User user, CancellationToken cancellationToken)
    {
        var claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),

        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claim,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiredMinutesTime),
            signingCredentials: creds);

        var stringToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        return stringToken;
    }
}