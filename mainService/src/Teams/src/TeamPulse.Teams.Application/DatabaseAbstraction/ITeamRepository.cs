using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface ITeamRepository
{
    Task<Team?> GetTeamIdAsync(Guid teamId, CancellationToken cancellationToken);
}