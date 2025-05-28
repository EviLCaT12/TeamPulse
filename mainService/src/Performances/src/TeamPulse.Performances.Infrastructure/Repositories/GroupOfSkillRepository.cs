using Microsoft.EntityFrameworkCore;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories;

public class GroupOfSkillRepository : IGroupOfSkillRepository
{
    private readonly WriteDbContext _writeDbContext;

    public GroupOfSkillRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddAsync(GroupOfSkills groupOfSkills, CancellationToken cancellationToken)
    { 
        await _writeDbContext.GroupOfSkills.AddAsync(groupOfSkills, cancellationToken);
    }

    public async Task<GroupOfSkills?> GetByIdAsync(GroupOfSkillsId id, CancellationToken cancellationToken)
    {
        var group = await _writeDbContext.GroupOfSkills
            .Include(g => g.SkillGrade)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        
        return group;
    }
}