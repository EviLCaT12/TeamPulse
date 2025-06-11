using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;

public interface IGroupSkillWriteRepository
{
    Task AddGroupSkillAsync(GroupSkill groupSkill, CancellationToken cancellationToken);
    
    Task<GroupSkill?> GetByIdAsync(
        GroupOfSkillsId groupId,
        SkillId skillId,
        CancellationToken cancellationToken);
}