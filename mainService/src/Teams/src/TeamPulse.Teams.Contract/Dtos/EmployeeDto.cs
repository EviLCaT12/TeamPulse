namespace TeamPulse.Teams.Contract.Dtos;

public class EmployeeDto
{
    public Guid Id { get; init; }
    
    public Guid? ManagedDepartmentId { get; init; }
    
    public Guid? ManagedTeamId { get; init; }
    public Guid? TeamId { get; init; }

}