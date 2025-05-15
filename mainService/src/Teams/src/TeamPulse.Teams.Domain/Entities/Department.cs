using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Department : Entity<DepartmentId>
{
    //efcore
    private Department()
    {
    }

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

    public void AddHeadOfDepartment(Employee employee)
    {
        HeadOfDepartment = employee;
    }

    public void RemoveTeam(Team team)
    {
        _teams.Remove(team);
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

    public UnitResult<Error> AddTeamEmployees(TeamId teamId, IEnumerable<Employee> employee)
    {
        var team = _teams.FirstOrDefault(t => t.Id == teamId);
        if (team is null)
            return Errors.General
                .ValueNotFound($"There is no team with id {teamId.Value} for department {Name.Value}.");
        
        team.AddEmployee(employee);
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> AddHeadOfTeam(TeamId teamId, Employee employee)
    {
        var team = _teams.FirstOrDefault(t => t.Id == teamId);
        if (team is null)
            return Errors.General
                .ValueNotFound($"There is no team with id {teamId.Value} for department {Name.Value}.");
        
        team.AddHeadOfTeam(employee);
        return UnitResult.Success<Error>();
    }
    
    public void UpdateHeadOfDepartment(Employee newHeadOfDepartment)
    {
        HeadOfDepartment = newHeadOfDepartment;
    }

    public UnitResult<Error> UpdateTeamName(TeamId teamId, Name newName)
    {
        var team = _teams.FirstOrDefault(t => t.Id == teamId);
        if (team is null)
            return Errors.General
                .ValueNotFound($"There is no team with id {teamId.Value} for department {Name.Value}.");
        
        team.UpdateName(newName);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateTeamEmployees(TeamId teamId, IEnumerable<Employee> newEmployees)
    {
        var team = _teams.FirstOrDefault(t => t.Id == teamId);
        if (team is null)
            return Errors.General
                .ValueNotFound($"There is no team with id {teamId.Value} for department {Name.Value}.");
        
        team.UpdateEmployees(newEmployees);
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateHeadOfTeam(TeamId teamId, Employee employee)
    {
        var team = _teams.FirstOrDefault(t => t.Id == teamId);
        if (team is null)
            return Errors.General
                .ValueNotFound($"There is no team with id {teamId.Value} for department {Name.Value}.");

        team.UpdateHeadOfTeam(employee);
        return UnitResult.Success<Error>();
    }
    
}