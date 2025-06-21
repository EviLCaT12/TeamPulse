using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TeamPulse.Accounts.Application.AccountManagers;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Infrastructure.Contexts;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Infrastructure.Managers;

public class RefreshSessionManager : IRefreshSessionManager
{
    private readonly WriteDbContext _context;

    public RefreshSessionManager(WriteDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(Guid refreshToken,
        CancellationToken cancellationToken)
    {
        var refreshSession = await _context.RefreshSessions
            .Include(rs => rs.User)
            .FirstOrDefaultAsync(r => r.RefreshTokenId == refreshToken, cancellationToken);
        if (refreshSession is null)
        {
            return Errors.General.ValueNotFound("Refresh Token not found");
        }

        return refreshSession;
    }

    public void DeleteRefreshSession(RefreshSession refreshSession)
    {
        _context.RefreshSessions.Remove(refreshSession);
    }
}