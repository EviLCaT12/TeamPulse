using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface IGroupOfSkillRepository
{
    Task AddAsync(GroupOfSkills groupOfSkills, CancellationToken cancellationToken);
}