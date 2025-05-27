using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories;

public class SkillGradeRepository : ISkillGradeRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SkillGradeRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddAsync(BaseSkillGrade skillGrade, CancellationToken cancellationToken)
    {
        await _writeDbContext.SkillGrades.AddAsync(skillGrade, cancellationToken);
    }
}