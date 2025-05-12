using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface IDepartmentRepository
{
    Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);
}