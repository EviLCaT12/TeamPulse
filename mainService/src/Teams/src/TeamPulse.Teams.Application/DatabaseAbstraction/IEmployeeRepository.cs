using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(EmployeeId? id, CancellationToken cancellationToken);
    
    Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    
    Task<List<Employee>> GetAllEmployeesFromTeamAsync(TeamId id, CancellationToken cancellationToken);
}