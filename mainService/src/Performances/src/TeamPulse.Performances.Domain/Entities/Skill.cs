using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities;

public class Skill : Entity<SkillId>
{
    //For ef core
    private Skill() {}

    public Skill(
        SkillId id,
        BaseSkillGrade skillGrade,
        Name name,
        Description description)
    {
        Id = id;
        SkillGrade = skillGrade;
        Name = name;
        Description = description;
    }
    
    public SkillId Id { get; private set; }
    
    public BaseSkillGrade SkillGrade { get; private set; }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }
    
}