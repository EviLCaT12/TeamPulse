using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;

public interface IGroupOfSkillReadRepository
{
    IQueryable<GroupOfSkillsDto> GetGroupOfSkills();
}