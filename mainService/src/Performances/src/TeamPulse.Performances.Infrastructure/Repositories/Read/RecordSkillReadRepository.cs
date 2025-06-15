using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Read;

public class RecordSkillReadRepository : IRecordSkillReadRepository
{
    private readonly ReadDbContext _context;

    public RecordSkillReadRepository(ReadDbContext context)
    {
        _context = context;
    }
    public IQueryable<RecordSkillDto> GetRecordSkills()
    {
        return _context.RecordSkills;
    }
}