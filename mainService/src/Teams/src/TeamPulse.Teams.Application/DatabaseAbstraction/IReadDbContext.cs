using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface IReadDbContext
{
    IQueryable<DepartmentDto> Departments { get; }
    
    IQueryable<TeamDto> Teams { get; }
    
    IQueryable<EmployeeDto> Employees { get; }
}