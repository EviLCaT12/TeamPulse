using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Read;

public class GroupSkillReadRepository : IGroupSkillReadRepository
{
    private readonly ReadDbContext _context;

    public GroupSkillReadRepository(ReadDbContext context)
    {
        _context = context;
    }

    public IQueryable<GroupSkillDto> GetGroupSkills()
    {
        return _context.GroupSkills;
    }
}