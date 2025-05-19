namespace TeamPulse.Teams.Contract.Dtos;

public class TeamDto
{
    public Guid Id { get; init; }
    
    public EmployeeDto[] Employees { get; init; }
    
    public Guid DepartmentId { get; init; }
    
    public string Name { get; init; }
    
    public Guid? HeadOfTeam { get; init; }
}