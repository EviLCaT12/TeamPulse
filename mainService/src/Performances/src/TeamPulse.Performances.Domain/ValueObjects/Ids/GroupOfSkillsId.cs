using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.ValueObjects.Ids;

public class GroupOfSkillsId : ValueObject
{
    //For ef core
    private GroupOfSkillsId()
    {
    }

    private GroupOfSkillsId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; }

    public static Result<GroupOfSkillsId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsInvalid("Value for group of skill identifier is empty.");

        return new GroupOfSkillsId(id);
    }

    public static GroupOfSkillsId NewId() => new GroupOfSkillsId(Guid.NewGuid());
    
    public static GroupOfSkillsId Empty() => new GroupOfSkillsId(Guid.Empty);


    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}