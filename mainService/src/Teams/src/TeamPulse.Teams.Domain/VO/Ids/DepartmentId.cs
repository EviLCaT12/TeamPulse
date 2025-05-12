using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Teams.Domain.VO.Ids;

public class DepartmentId : ValueObject
{
    //efcore
    private DepartmentId() { }
    
    private DepartmentId(Guid id) => Value = id;
    public Guid Value { get; }

    public static Result<DepartmentId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsRequired("Id for Department cannot be empty.");

        return new DepartmentId(id);
    }

    public static DepartmentId CreateNewId() => new(Guid.NewGuid());
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}