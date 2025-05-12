using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.ValueObjects.Ids;

public class SkillIds : ValueObject
{
    //For ef core
    private SkillIds()
    {
    }

    private SkillIds(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; }

    public static Result<SkillIds, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsInvalid("Value for skill identifier is empty.");

        return new SkillIds(id);
    }

    public static SkillIds NewId() => new SkillIds(Guid.NewGuid());
    
    public static SkillIds Empty() => new SkillIds(Guid.Empty);


    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}