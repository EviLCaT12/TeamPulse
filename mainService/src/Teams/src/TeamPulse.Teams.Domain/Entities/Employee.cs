using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Employee : Entity<EmployeeId> 
{
    //efcore
    private Employee() {}

    private Employee(EmployeeId id)
    {
        Id = id;
    }
    
    public EmployeeId Id { get; private set; }
    
    public Team Team { get; private set; }
    public Team ManagedTeam { get; private set; }
    public Department Department { get; private set; }
    
    public Guid DepartmentId { get; private set; }

    public static Result<Employee, Error> Create(EmployeeId id, Guid teamId)
    {
        return new Employee(id);
    }
}