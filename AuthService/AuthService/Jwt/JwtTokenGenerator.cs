using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Domain;
using AuthService.Errors;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;


namespace AuthService.Jwt;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly JwtOptions _options;

    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public Result<string, Error> GenerateToken(User user)
    {
        Claim[] claims =
        [
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new (JwtRegisteredClaimNames.EmailVerified, user.EmailConfirmed.ToString()),
            new (JwtRegisteredClaimNames.Nickname, user.UserName ?? "")
        ];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        
        var signedCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var jwtToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.TokenLifetimeInMinute),
            signingCredentials: signedCredential);
        
        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return jwtStringToken;
    }
}