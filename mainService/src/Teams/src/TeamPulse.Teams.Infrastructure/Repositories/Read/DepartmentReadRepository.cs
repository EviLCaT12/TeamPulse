using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.Infrastructure.Repositories.Read;

public class DepartmentReadRepository : IDepartmentReadRepository
{
    private readonly ReadDbContext _dbContext;

    public DepartmentReadRepository(ReadDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<DepartmentDto> GetDepartments()
    {
        return _dbContext.Departments;
    }
}