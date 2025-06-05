namespace TeamPulse.Performances.Contract.Dtos;

public class SkillDto
{
    public Guid Id { get; init; }
    
    public SkillGradeDto Grade { get; init; }
    
    public string Name { get; init; }
    
    public string Description { get; init; }
}