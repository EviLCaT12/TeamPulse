namespace TeamPulse.Teams.Contract.Dtos;

public class DepartmentDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; }
    
    public TeamDto[] Teams { get; init; }
    
    public Guid? HeadOfDepartment { get; init; }
}