namespace TeamPulse.Teams.Contract.Dtos;

public class DepartmentDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    public EmployeeDto HeadOfDepartment { get; init; }
    public IReadOnlyList<TeamDto> Teams { get; init; }
    
    public IReadOnlyList<EmployeeDto> Employees { get; init; }
}