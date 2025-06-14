namespace TeamPulse.Teams.Contract.Dtos;

public class TeamDto
{
    public Guid Id { get; init; }
    public Guid DepartmentId { get; init; }
    public string Name { get; init; } 
    
    
    public EmployeeDto? HeadOfTeam { get; init; }
    public IReadOnlyList<EmployeeDto> Employees { get; init; }
}