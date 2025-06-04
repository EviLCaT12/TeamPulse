using Microsoft.EntityFrameworkCore;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories;

public class TeamRepository: ITeamRepository
{
    private readonly WriteDbContext _writeDbContext;

    public TeamRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task<Team?> GetTeamIdAsync(TeamId teamId, CancellationToken cancellationToken)
    {
        return await _writeDbContext.Teams
            .Include(t => t.DepartmentId.Value)
            .FirstOrDefaultAsync(t => t.Id == teamId, cancellationToken);
    }

    public async Task AddTeamAsync(Team team, CancellationToken cancellationToken)
    {
        await _writeDbContext.Teams.AddAsync(team, cancellationToken);
    }

    public void DeleteTeam(Team team)
    {
        _writeDbContext.Teams.Remove(team);
    }
}