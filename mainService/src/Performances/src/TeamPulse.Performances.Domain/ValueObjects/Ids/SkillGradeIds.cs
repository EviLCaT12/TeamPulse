using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.ValueObjects.Ids;

public class SkillGradeIds : ValueObject
{
    //For ef core
    private SkillGradeIds()
    {
    }

    private SkillGradeIds(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; }

    public static Result<SkillGradeIds, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsInvalid("Value for skill identifier is empty.");

        return new SkillGradeIds(id);
    }

    public static SkillGradeIds NewId() => new SkillGradeIds(Guid.NewGuid());
    
    public static SkillGradeIds Empty() => new SkillGradeIds(Guid.Empty);


    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}