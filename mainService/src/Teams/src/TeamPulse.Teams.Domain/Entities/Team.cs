using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Team : Entity<TeamId>
{
    //ef core
    private Team()
    {
    }

    public Team(
        TeamId id,
        Name name,
        Department department,
        IEnumerable<Employee>? employees,
        Employee? headOfTeam)
    {
        Id = id;
        _employees = employees?.ToList() ?? [];
        Name = name;
        Department = department;
        HeadOfTeam = headOfTeam;
    }

    public TeamId Id { get; private set; }

    private List<Employee> _employees = [];

    public IReadOnlyList<Employee> Employees => _employees; 

    public Department Department { get; private set; }

    public Name Name { get; private set; }

    public Employee? HeadOfTeam { get; private set; }
    
}