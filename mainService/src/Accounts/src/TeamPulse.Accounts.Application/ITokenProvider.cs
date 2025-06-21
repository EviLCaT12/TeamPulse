using System.Security.Claims;
using CSharpFunctionalExtensions;
using TeamPulse.Accounts.Application.Models;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Application;

public interface ITokenProvider
{
    Task<JwtTokenResult> GenerateAccessTokenAsync(User user, CancellationToken cancellationToken);
    Task<Guid> GenerateRefreshTokenAsync(User user, Guid accessTokenJti, CancellationToken cancellationToken);

    Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken, CancellationToken cancellationToken);
}