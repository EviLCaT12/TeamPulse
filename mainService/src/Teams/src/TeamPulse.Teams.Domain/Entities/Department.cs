using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Department : Entity<DepartmentId>
{
    //efcore
    private Department() {}

    public Department(
        DepartmentId id,
        Name name,
        IEnumerable<Team>? teams,
        Employee? headOfDepartment)
    {
        Id = id;
        Name = name;
        _teams = teams?.ToList() ?? [];
        HeadOfDepartment = headOfDepartment;
    }

    public DepartmentId Id { get; private set; }
    
    public Name Name { get; private set; }

    private List<Team> _teams = [];

    public IReadOnlyList<Team> Teams => _teams;
    
    public Employee? HeadOfDepartment { get; private set; }

    public void AddTeams(IEnumerable<Team> teams)
    {
        _teams.AddRange(teams);
    }

    public void UpdateName(Name newName)
    {
        Name = newName;
    }

    public void UpdateTeams(IEnumerable<Team> teams)
    {
        _teams.Clear();
        _teams.AddRange(teams);
    }

    public void UpdateHeadOfDepartment(Employee newHeadOfDepartment)
    {
        HeadOfDepartment = newHeadOfDepartment;
    }
}