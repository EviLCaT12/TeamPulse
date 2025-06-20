using Microsoft.EntityFrameworkCore;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories.Write;

public class DepartmentWriteRepository : IDepartmentWriteRepository
{
    private readonly WriteDbContext _context;

    public DepartmentWriteRepository(WriteDbContext context)
    {
        _context = context;
    }
    public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken)
    {
        await _context.Departments.AddAsync(department, cancellationToken);
    }

    public async Task<Department?> GetDepartmentByIdAsync(DepartmentId departmentId, CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Include(d => d.Teams)
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);
    }

    public void DeleteDepartment(Department department)
    {
        _context.Departments.Remove(department);
    }
}