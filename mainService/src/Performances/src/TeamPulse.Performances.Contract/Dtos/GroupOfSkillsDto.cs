namespace TeamPulse.Performances.Contract.Dtos;

public class GroupOfSkillsDto
{
    public Guid Id { get; init; }
    
    public string GroupName { get; init; }
    
    public string GroupDescription { get; init; }
    
    public SkillGradeDto SkillGrade { get; init; }
}