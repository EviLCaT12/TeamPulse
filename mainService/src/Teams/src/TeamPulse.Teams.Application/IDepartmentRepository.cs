using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.Application;

public interface IDepartmentRepository
{
    Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);
}