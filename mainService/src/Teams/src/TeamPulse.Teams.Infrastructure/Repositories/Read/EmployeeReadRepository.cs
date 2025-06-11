using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories.Read;

public class EmployeeReadRepository : IEmployeeReadRepository
{
    private readonly ReadDbContext _context;

    public EmployeeReadRepository(ReadDbContext context)
    {
        _context = context;
    }
    public IQueryable<EmployeeDto> GetEmployees()
    {
        return _context.Employees;
    }
}