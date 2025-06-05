using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface ITeamRepository
{
    Task<Team?> GetTeamByIdAsync(TeamId teamId, CancellationToken cancellationToken);
    
    Task AddTeamAsync(Team team, CancellationToken cancellationToken);
    
    void DeleteTeam(Team team);
}