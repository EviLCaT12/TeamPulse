using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface IGroupOfSkillRepository
{
    Task AddAsync(GroupOfSkills groupOfSkills, CancellationToken cancellationToken);
    
    Task<GroupOfSkills?> GetByIdAsync(GroupOfSkillsId id, CancellationToken cancellationToken);
}