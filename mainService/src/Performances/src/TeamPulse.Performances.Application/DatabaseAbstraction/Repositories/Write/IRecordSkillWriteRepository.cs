using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;

public interface IRecordSkillWriteRepository
{
    Task AddRecordSkillAsync(RecordSkill skill, CancellationToken cancellationToken);
}