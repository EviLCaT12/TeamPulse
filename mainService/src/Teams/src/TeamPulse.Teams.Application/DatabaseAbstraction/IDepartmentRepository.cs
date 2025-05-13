using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.DatabaseAbstraction;

public interface IDepartmentRepository
{
    Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);

    Task<Department?> GetDepartmentByIdAsync(DepartmentId departmentId, CancellationToken cancellationToken);
}