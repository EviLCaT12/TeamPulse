using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Write;

public class RecordSkillWriteRepository : IRecordSkillWriteRepository
{
    private readonly WriteDbContext _writeDbContext;

    public RecordSkillWriteRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddRecordSkillAsync(RecordSkill skill, CancellationToken cancellationToken)
    {
        await _writeDbContext.RecordSkills.AddAsync(skill, cancellationToken);
    }
}