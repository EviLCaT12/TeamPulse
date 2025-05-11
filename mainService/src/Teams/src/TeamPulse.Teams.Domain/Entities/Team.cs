using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Team : Entity<TeamId>
{
    //ef core
    private Team() {}

    private Team(
        TeamId id,
        IEnumerable<Guid> employees,
        Guid departmentId,
        Name name,
        Guid headOfTeam)
    {
        Id = id;
        _employees = employees.ToList();
        DepartmentId = departmentId;
        Name = name;
        HeadOfTeam = headOfTeam;
    }
    
    public TeamId Id { get; private set; }

    private List<Guid> _employees = [];
    
    public IReadOnlyList<Guid> Employees => _employees;
    
    public Guid DepartmentId { get; private set; }
    
    public Name Name { get; private set; }
    
    public Guid HeadOfTeam { get; private set; }

    public static Result<Team, Error> Create(
        TeamId id,
        List<Guid> employees,
        Guid departmentId,
        Name name,
        Guid headOfTeam)
    {
        if (employees.Count != 0)
            return Errors.General.ValueIsRequired("Cannot create a team without any employees");
        
        if (headOfTeam == Guid.Empty)
            return Errors.General.ValueIsRequired("Cannot create a team without a head of team");
        
        if (departmentId == Guid.Empty)
            return Errors.General.ValueIsRequired("Cannot create a team without a department");
        
        return new Team(id, employees, departmentId, name, headOfTeam);
    }
}