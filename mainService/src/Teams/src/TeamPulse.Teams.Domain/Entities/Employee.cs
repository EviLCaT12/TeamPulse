using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Employee : Entity<EmployeeId>
{
    //efcore
    private Employee()
    {
    }

    public Employee(EmployeeId id)
    {
        Id = id;
    }

    public EmployeeId Id { get; private set; }
    public TeamId? TeamId { get; private set; }
    public DepartmentId? DepartmentId { get; private set; }

    public bool IsTeamManager { get; private set; } = false;

    public bool IsDepartmentManager { get; private set; } = false;

    internal void SetTeamManager()
    {
        IsTeamManager = true;
    }

    internal void RemoveFromTeamManager()
    {
        IsTeamManager = false;
    }

    internal void SetDepartmentManager()
    {
        IsDepartmentManager = true;
    }
    
    internal void RemoveFromDepartmentManager()
    {
        IsDepartmentManager = false;
    }
}