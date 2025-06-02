using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.DatabaseAbstraction.Repositories;

public interface ISkillRepository
{
    Task AddSkillAsync(Skill skill, CancellationToken cancellationToken);
    
    Task<Skill?> GetByIdAsync(SkillId id, CancellationToken cancellationToken);
}