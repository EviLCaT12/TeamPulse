using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(Guid? id, CancellationToken cancellationToken);
}