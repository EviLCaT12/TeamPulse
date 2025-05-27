using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface ISkillRepository
{
    Task AddSkillAsync(Skill skill, CancellationToken cancellationToken);
}