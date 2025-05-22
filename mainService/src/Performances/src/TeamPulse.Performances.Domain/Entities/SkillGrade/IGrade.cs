

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public interface IGrade<T> : IGradeBase
{
    IReadOnlyList<T> Grades { get; }
}