using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Teams.Domain.VO.Ids;

public class EmployeeId : ValueObject
{
    //efcore
    private EmployeeId() { }
    
    private EmployeeId(Guid id) => Value = id;
    public Guid Value { get; }

    public static Result<EmployeeId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsRequired("Id for employee cannot be empty.");

        return new EmployeeId(id);
    }

    public static EmployeeId CreateNewId() => new(Guid.NewGuid());
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}