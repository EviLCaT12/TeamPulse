using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Teams.Domain.VO.Ids;

public class TeamId : ValueObject
{
    //efcore
    private TeamId() { }
    
    private TeamId(Guid id) => Value = id;
    public Guid Value { get; }

    public static Result<TeamId, Error> Create(Guid id)
    {
        if (id == Guid.Empty)
            return Errors.General.ValueIsRequired("Id for team cannot be empty.");

        return new TeamId(id);
    }

    public static TeamId CreateNewId() => new(Guid.NewGuid());
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}