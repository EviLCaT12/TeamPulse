using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Constants;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.SharedKernel.SharedVO;

public class Description : ValueObject
{
    //ef core
    private Description()
    {
    }

    private Description(string description)
    {
        Value = description;
    }

    public string Value { get; }

    public static Result<Description, Error> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.Errors.General.ValueIsRequired("Description is required");

        if (description.Length > DescriptionConstant.MAX_LENGTH)
            return Errors.Errors.General
                .ValueIsInvalid($"Description must be between 2 and {DescriptionConstant.MAX_LENGTH} symbols");

        return new Description(description);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}