using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Application.DatabaseAbstraction.Repositories;

public interface IRecordSkillRepository
{
    Task AddRecordSkillAsync(RecordSkill skill, CancellationToken cancellationToken);
}