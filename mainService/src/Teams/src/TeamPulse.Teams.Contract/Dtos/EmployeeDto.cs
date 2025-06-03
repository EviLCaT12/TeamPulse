namespace TeamPulse.Teams.Contract.Dtos;

//ToDo: Вообще не нравится как сделано, в ответе Team приходится делать null, что костыль. Подумать, как сделать лучше.
public class EmployeeDto
{
    public Guid Id { get; init; }
    
    public Guid? ManagedDepartmentId { get; init; }
    
    public Guid DepartmentId { get; init; }
    public Guid? ManagedTeamId { get; init; }
    
    public TeamDto Team { get; init; }
    
    public Guid? TeamId { get; init; }

}