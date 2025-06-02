using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories;

public class RecordSkillRepository : IRecordSkillRepository
{
    private readonly WriteDbContext _writeDbContext;

    public RecordSkillRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddRecordSkillAsync(RecordSkill skill, CancellationToken cancellationToken)
    {
        await _writeDbContext.RecordSkills.AddAsync(skill, cancellationToken);
    }
}