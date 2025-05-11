using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Constants;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.SharedKernel.SharedVO;

public class Name : ValueObject
{
    //ef core
    private Name()
    {
    }

    private Name(string name)
    {
        Value = name;
    }

    public string Value { get; }

    public static Result<Name, Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.Errors.General.ValueIsRequired("Name is required");

        if (name.Length > NameConstant.MAX_LENGTH)
            return Errors.Errors.General
                .ValueIsInvalid($"Name must be between 2 and {NameConstant.MAX_LENGTH} symbols");

        return new Name(name);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}