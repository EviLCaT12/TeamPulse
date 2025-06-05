using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities;


/// <summary>
/// По факту, это просто набор скиллов, но решено было вынести их в отдельную сущность
/// так как предвидятся некоторые особые валидации и методы взаимодействия.
/// </summary>
public class GroupOfSkills : Entity<GroupOfSkillsId>
{
    //ef core
    private GroupOfSkills() { }

    //Добавлять скиллы будет отдельный метод
    public GroupOfSkills(
        GroupOfSkillsId id,
        Name name,
        Description description,
        BaseSkillGrade skillGrade)
    {
        Id = id;
        Name = name;
        Description = description;
        SkillGrade = skillGrade;
    }
    
    public GroupOfSkillsId Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }
    
    public BaseSkillGrade SkillGrade { get; private set; }
}