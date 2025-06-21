using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TeamPulse.Accounts.Application;
using TeamPulse.Accounts.Application.Models;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Infrastructure.Contexts;
using TeamPulse.Accounts.Infrastructure.Options;
using TeamPulse.Framework.Authorization;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Infrastructure.Jwt;

public class JwtTokenProvider : ITokenProvider 
{
    private readonly WriteDbContext _context;
    private readonly JwtOptions _options;

    public JwtTokenProvider(
        IOptions<JwtOptions> options,
        WriteDbContext context)
    {
        _context = context;
        _options = options.Value;
    }
    public async Task<JwtTokenResult> GenerateAccessTokenAsync(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Roles)
            .Select(r => new Claim(ClaimTypes.Role, r.Name ?? string.Empty))
            .ToListAsync(cancellationToken);

        var jti = Guid.NewGuid();
        
        Claim[] claims = [
            new (CustomClaims.Id, user.Id.ToString()),
            new (CustomClaims.Jti, jti.ToString()),
            new (CustomClaims.Email, user.Email ?? string.Empty)
        ];
         
        claims = claims.Concat(roleClaims).ToArray();
        
        var jwtToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiredMinutesTime),
            signingCredentials: signingCredentials,
            claims: claims);
        
        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        
        return new JwtTokenResult(jwtStringToken, jti);
    }

    public async Task<Guid> GenerateRefreshTokenAsync(User user, Guid accessTokenJti,
        CancellationToken cancellationToken)
    {
        var refreshSession = new RefreshSession
        {
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(_options.ExpiredDaysTime),
            Jti = accessTokenJti,
            RefreshTokenId = Guid.NewGuid()
        };
        
        await _context.RefreshSessions.AddAsync(refreshSession, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return refreshSession.RefreshTokenId;
        
    }

    public async Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(
        string jwtToken, CancellationToken cancellationToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_options);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);

        if (validationResult.IsValid == false)
            return Errors.General.ValueIsInvalid("Token is invalid").ToErrorList();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}