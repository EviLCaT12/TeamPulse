using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.ValueObjects.Ids;

public class SkillGradeId : ValueObject
{
    //For ef core
    private SkillGradeId()
    {
    }

    private SkillGradeId(Guid id)
    {
        Value = id;
    }

    public Guid Value { get; }

    public static Result<SkillGradeId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsInvalid("Value for skill identifier is empty.");

        return new SkillGradeId(id);
    }

    public static SkillGradeId NewId() => new SkillGradeId(Guid.NewGuid());
    
    public static SkillGradeId Empty() => new SkillGradeId(Guid.Empty);


    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}