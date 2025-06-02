namespace TeamPulse.Performances.Contract.Dtos;

public class SkillGradeDto
{
    public Guid Id { get; init; }
    
    public string Grades { get; init; }
    
    public string GradeType { get; init; }
    
    public string GradeName { get; init; }
    
    public string GradeDescription { get; init; }
}