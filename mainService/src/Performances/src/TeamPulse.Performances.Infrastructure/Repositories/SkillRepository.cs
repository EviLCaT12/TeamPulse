using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SkillRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddSkillAsync(Skill skill, CancellationToken cancellationToken)
    {
        await _writeDbContext.Skills.AddAsync(skill, cancellationToken);
    }
}