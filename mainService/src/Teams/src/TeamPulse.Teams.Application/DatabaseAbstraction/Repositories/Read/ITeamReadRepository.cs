using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;

public interface ITeamReadRepository
{
    IQueryable<TeamDto> GetTeams();
}