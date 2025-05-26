using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Domain.Entities;

/// <summary>
/// Данная сущность предназначена для хранения информации о том,
/// какая команда или сотрудник владеет данным скилом, его оценке
/// и оценке главного
/// </summary>
public class RecordSkill
{
    //ef core
    private RecordSkill() {}
    
    public Guid Id { get; private set; }
    
    //Либо команда, либо сотрудник - всё равно.
    public Guid RecordId { get; private set; }
    
    public string? SelfGrade { get; private set; }
    
    public string? ManagerGrade { get; private set; }
    
    public Skill Skill { get; private set; }
 }