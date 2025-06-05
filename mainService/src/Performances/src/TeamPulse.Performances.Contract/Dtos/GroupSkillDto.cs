namespace TeamPulse.Performances.Contract.Dtos;

public class GroupSkillDto
{
    public Guid GroupId { get; init; }
    
    public GroupOfSkillsDto Group { get; init; }
    
    public Guid SkillId { get; init; }
    
    public SkillDto Skill { get; init; }
}