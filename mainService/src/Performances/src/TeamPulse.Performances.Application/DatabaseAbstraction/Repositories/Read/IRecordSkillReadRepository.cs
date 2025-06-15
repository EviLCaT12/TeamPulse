using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;

public interface IRecordSkillReadRepository
{
    IQueryable<RecordSkillDto> GetRecordSkills();
}