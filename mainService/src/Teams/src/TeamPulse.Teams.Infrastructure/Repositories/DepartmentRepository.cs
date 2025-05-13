using Microsoft.EntityFrameworkCore;
using TeamPulse.Teams.Application;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly WriteDbContext _context;

    public DepartmentRepository(WriteDbContext context)
    {
        _context = context;
    }
    public async Task AddDepartmentAsync(Department department, CancellationToken cancellationToken)
    {
        await _context.Departments.AddAsync(department, cancellationToken);
    }

    public async Task<Department?> GetDepartmentByIdAsync(DepartmentId departmentId, CancellationToken cancellationToken)
    {
        return await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);
    }
}