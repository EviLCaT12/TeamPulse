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
        IEnumerable<Guid> teams,
        Guid headOfDepartment)
    {
        Id = id;
        Name = name;
        _teams = teams.ToList();
        HeadOfDepartment = headOfDepartment;
    }

    public DepartmentId Id { get; private set; }
    
    public Name Name { get; private set; }

    private List<Guid> _teams = [];

    public IReadOnlyList<Guid> Teams => _teams;
    
    public Guid HeadOfDepartment { get; private set; }

    public static Result<Department, Error> Create(
        DepartmentId id,
        Name name,
        List<Guid> teams,
        Guid headOfDepartment)
    {
        if (teams.Count != 0)
            return Errors.General.ValueIsRequired("Cannot create a department without any teams");
        
        if (headOfDepartment == Guid.Empty)
            return Errors.General.ValueIsRequired("Cannot create a department without a head of department");
        
        return new Department(id, name, teams, headOfDepartment);
    }
}