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
        IEnumerable<Employee> employees,
        Name name,
        Employee headOfTeam)
    {
        Id = id;
        _employees = employees.ToList();
        Name = name;
        HeadOfTeam = headOfTeam;
    }
    
    public TeamId Id { get; private set; }

    private List<Employee> _employees = [];
    
    public IReadOnlyList<Employee> Employees => _employees;
    
    public Department Department { get; private set; }
    
    
    public Name Name { get; private set; }
    
    public Employee HeadOfTeam { get; private set; }

    public static Result<Team, Error> Create(
        TeamId id,
        List<Employee> employees,
        Name name,
        Employee headOfTeam)
    {
        if (employees.Count != 0)
            return Errors.General.ValueIsRequired("Cannot create a team without any employees");
        
        return new Team(id, employees, name, headOfTeam);
    }
}