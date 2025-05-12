using Microsoft.EntityFrameworkCore;
using TeamPulse.Teams.Application;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly WriteDbContext _writeDbContext;

    public EmployeeRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task<Employee?> GetEmployeeByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        return await _writeDbContext.Employees.FirstOrDefaultAsync(e => e.Id.Value == id, cancellationToken);
    }
}