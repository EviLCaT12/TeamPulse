using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Domain.Entities;

/// <summary>
/// Табличка many-to-many.
/// Требуется для хранения инфы о том, каким навыком и как хорошо
/// обладает сотрудник 
/// </summary>
public class EmployeeSkill
{
    public Guid EmployeeId {get; private set;}
    
    public SkillId SkillId {get; private set;}
    
    public IGradeBase SelfGrade {get; private set;}
    
    public IGradeBase ManagerGrade {get; private set;}
    
 }