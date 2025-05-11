using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities;

public class Skill : Entity<SkillIds>
{
    //For ef core
    private Skill() {}
    
    public SkillIds Id { get; private set; }
    
    public Guid TeamId { get; private set; }
    
    public Guid SkillGradeId { get; private set; }
    
    public Name Name { get; private set; }
    
    public Description Description { get; private set; }
}