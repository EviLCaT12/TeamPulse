using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities;

public class GroupOfSkills
{
    public GroupOfSkillsId Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }

    private List<Skill> _skills = [];
    
    public IReadOnlyList<Skill> Skills => _skills;
}