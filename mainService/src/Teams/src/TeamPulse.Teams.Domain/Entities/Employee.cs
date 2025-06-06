using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Employee : Entity<EmployeeId> 
{
    //efcore
    private Employee() {}

    public Employee(EmployeeId id)
    {
        Id = id;
    }
    
    public EmployeeId Id { get; private set; }
    
    public Team Team { get; private set; }
    public Team? ManagedTeam { get; private set; }
    
    public Department? ManagedDepartment { get; private set; }
    public DepartmentId? ManagedDepartmentId { get; private set; }
    
}