using Microsoft.EntityFrameworkCore;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Write;

public class SkillWriteRepository : ISkillWriteRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SkillWriteRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddSkillAsync(Skill skill, CancellationToken cancellationToken)
    {
        await _writeDbContext.Skills.AddAsync(skill, cancellationToken);
    }

    public async Task<Skill?> GetByIdAsync(SkillId id, CancellationToken cancellationToken)
    {
        return await _writeDbContext.Skills
            .Include(s => s.SkillGrade)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken); 
    }
}