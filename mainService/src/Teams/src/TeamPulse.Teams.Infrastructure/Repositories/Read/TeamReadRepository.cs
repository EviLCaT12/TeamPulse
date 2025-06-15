using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories.Read;

public class TeamReadRepository : ITeamReadRepository
{
    private readonly ReadDbContext _context;

    public TeamReadRepository(ReadDbContext context)
    {
        _context = context;
    }
    public IQueryable<TeamDto> GetTeams()
    {
        return _context.Teams;
    }
}