using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Write;

public class GroupSkillWriteRepository : IGroupSkillWriteRepository
{
    private readonly WriteDbContext _writeDbContext;

    public GroupSkillWriteRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task AddGroupSkillAsync(GroupSkill groupSkill, CancellationToken cancellationToken)
    {
        await _writeDbContext.GroupSkills.AddAsync(groupSkill, cancellationToken);
    }

    public async Task<GroupSkill?> GetByIdAsync(
        GroupOfSkillsId groupId,
        SkillId skillId, 
        CancellationToken cancellationToken)
    {
        return await _writeDbContext.GroupSkills.FindAsync([groupId, skillId], cancellationToken);
    }
}