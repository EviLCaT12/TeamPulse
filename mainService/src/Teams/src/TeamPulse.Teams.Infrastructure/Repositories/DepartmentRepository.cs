using TeamPulse.Teams.Application;
using TeamPulse.Teams.Domain.Entities;
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
}