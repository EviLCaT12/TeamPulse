using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.ValueObjects.Ids;

public class SkillId : ValueObject
{
    //For ef core
    private SkillId()
    {
    }

    private SkillId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; }

    public static Result<SkillId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsInvalid("Value for skill identifier is empty.");

        return new SkillId(id);
    }

    public static SkillId NewId() => new SkillId(Guid.NewGuid());
    
    public static SkillId Empty() => new SkillId(Guid.Empty);


    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}