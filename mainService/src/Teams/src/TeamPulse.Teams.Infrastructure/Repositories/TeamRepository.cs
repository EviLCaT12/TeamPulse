using Microsoft.EntityFrameworkCore;
using TeamPulse.Teams.Application;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories;

public class TeamRepository: ITeamRepository
{
    private readonly WriteDbContext _writeDbContext;

    public TeamRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task<Team?> GetTeamIdAsync(Guid teamId, CancellationToken cancellationToken)
    {
        return await _writeDbContext.Teams.FirstOrDefaultAsync(t => t.Id.Value == teamId, cancellationToken);
    }
}