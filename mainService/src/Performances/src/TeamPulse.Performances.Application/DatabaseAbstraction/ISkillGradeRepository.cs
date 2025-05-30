using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface ISkillGradeRepository
{ 
    Task AddAsync(BaseSkillGrade skillGrade, CancellationToken cancellationToken);
    
    Task<BaseSkillGrade?> GetByIdAsync(SkillGradeId id, CancellationToken cancellationToken);
}   