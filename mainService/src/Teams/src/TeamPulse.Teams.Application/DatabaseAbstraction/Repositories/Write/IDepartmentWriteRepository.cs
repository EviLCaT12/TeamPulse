using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Write;

public interface IDepartmentWriteRepository
{
    Task AddDepartmentAsync(Department department, CancellationToken cancellationToken);

    Task<Department?> GetDepartmentByIdAsync(DepartmentId departmentId, CancellationToken cancellationToken);
    
    void DeleteDepartment(Department department);
}