using Microsoft.EntityFrameworkCore;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories;

public class EmployeeWriteRepository : IEmployeeWriteRepository
{
    private readonly WriteDbContext _writeDbContext;

    public EmployeeWriteRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task<Employee?> GetEmployeeByIdAsync(EmployeeId? id, CancellationToken cancellationToken)
    {
        return await _writeDbContext.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        await _writeDbContext.Employees.AddAsync(employee, cancellationToken);
    }

    public async Task<List<Employee>> GetAllEmployeesFromTeamAsync(TeamId id, CancellationToken cancellationToken)
    {
        var employees = await _writeDbContext.Employees
            .Where(e => e.WorkingTeamId == id)
            .ToListAsync(cancellationToken);
        
        return employees;
    }
}