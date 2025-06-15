using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.Performances.Infrastructure.DbContexts;

namespace TeamPulse.Performances.Infrastructure.Repositories.Write;

public class SkillGradeWriteRepository : ISkillGradeWriteRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SkillGradeWriteRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }
    public async Task AddAsync(BaseSkillGrade skillGrade, CancellationToken cancellationToken)
    {
        await _writeDbContext.SkillGrades.AddAsync(skillGrade, cancellationToken);
    }

    //Вначале будем искать в change tracker`у, уже после запрос в бд
    public async Task<BaseSkillGrade?> GetByIdAsync(SkillGradeId id, CancellationToken cancellationToken)
    {
        return await _writeDbContext.SkillGrades.FindAsync([id], cancellationToken);
    }
}