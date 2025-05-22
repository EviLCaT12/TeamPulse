using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public interface IGradeBase
{
    SkillGradeId Id { get; } 
    
    IReadOnlyList<string> GetGradesAsString();
}