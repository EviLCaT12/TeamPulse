using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface IGroupSkillRepository
{
    Task<GroupSkill?> GetByIdAsync(
        GroupOfSkillsId groupId,
        SkillId skillId,
        CancellationToken cancellationToken);
}