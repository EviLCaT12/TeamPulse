using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface IRecordSkillRepository
{
    Task AddRecordSkillAsync(RecordSkill skill, CancellationToken cancellationToken);
}