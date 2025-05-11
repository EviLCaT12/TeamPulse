using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Domain.Entities;

public class Department : Entity<DepartmentId>
{
    //efcore
    private Department() {}

    private Department(
        DepartmentId id,
        Name name,
        IEnumerable<Team> teams,
        Employee headOfDepartment)
    {
        Id = id;
        Name = name;
        _teams = teams.ToList();
        HeadOfDepartment = headOfDepartment;
    }

    public DepartmentId Id { get; private set; }
    
    public Name Name { get; private set; }

    private List<Team> _teams = [];

    public IReadOnlyList<Team> Teams => _teams;
    
    public Employee HeadOfDepartment { get; private set; }

    public static Result<Department, Error> Create(
        DepartmentId id,
        Name name,
        List<Team> teams,
        Employee headOfDepartment)
    {
        if (teams.Count != 0)
            return Errors.General.ValueIsRequired("Cannot create a department without any teams");
        
        return new Department(id, name, teams, headOfDepartment);
    }
}