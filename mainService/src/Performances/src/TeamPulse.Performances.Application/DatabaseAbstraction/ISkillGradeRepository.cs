using TeamPulse.Performances.Domain.Entities.SkillGrade;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface ISkillGradeRepository
{ 
    Task AddAsync(BaseSkillGrade skillGrade, CancellationToken cancellationToken);
}