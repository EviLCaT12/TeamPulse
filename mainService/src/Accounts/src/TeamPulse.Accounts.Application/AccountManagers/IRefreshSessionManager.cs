using CSharpFunctionalExtensions;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Application.AccountManagers;

public interface IRefreshSessionManager
{
    Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken,
        CancellationToken cancellationToken);
    
    void DeleteRefreshSession(RefreshSession refreshSession);
}