using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Read;

public class GroupOfSkillReadRepository : IGroupOfSkillReadRepository
{
    private readonly ReadDbContext _dbContext;

    public GroupOfSkillReadRepository(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<GroupOfSkillsDto> GetGroupOfSkills()
    {
        return _dbContext.GroupOfSkills;
    }
}