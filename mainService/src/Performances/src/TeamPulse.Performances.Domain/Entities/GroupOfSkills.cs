using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities;


/// <summary>
/// По факту, это просто набор скиллов, но решено было вынести их в отдельную сущность
/// так как предвидятся некоторые особые валидации и методы взаимодействия.
/// </summary>
public class GroupOfSkills : Entity<GroupOfSkills>
{
    //ef core
    private GroupOfSkills() { }
    
    public GroupOfSkillsId Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }

    private List<Skill> _skills = [];
    
    public IReadOnlyList<Skill> Skills => _skills;
}