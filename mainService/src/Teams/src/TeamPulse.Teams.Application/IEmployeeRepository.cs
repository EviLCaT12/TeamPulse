using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.Application;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(Guid? id, CancellationToken cancellationToken);
}