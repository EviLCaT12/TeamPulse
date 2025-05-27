using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public abstract class BaseSkillGrade : IGradeBase
{
    public SkillGradeId Id { get; protected set; }
    
    public string GradesAsString { get; protected set; }
    
    public Name Name { get; protected set; }
    
    public Description Description { get; protected set; }
    
    public abstract IReadOnlyList<string> GetGradesAsString();
    
}