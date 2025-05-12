using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Employee : Entity<EmployeeId> 
{
    //efcore
    private Employee() {}

    private Employee(EmployeeId id, Guid teamId)
    {
        Id = id;
        TeamId = teamId;
    }
    
    public EmployeeId Id { get; private set; }
    
    public Guid TeamId { get; private set; }

    public static Result<Employee, Error> Create(EmployeeId id, Guid teamId)
    {
        if (teamId == Guid.Empty)
            return Errors.General.ValueIsRequired("Cannot create an employee without a team");
        
        return new Employee(id, teamId);
    }
}