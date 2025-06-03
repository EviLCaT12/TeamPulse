using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Application.DatabaseAbstraction;

public interface IReadDbContext
{
    IQueryable<SkillGradeDto> SkillGrades { get; }
    
    IQueryable<GroupOfSkillsDto> GroupOfSkills { get; }
    
    IQueryable<GroupSkillDto> GroupSkills { get; }
    
    IQueryable<SkillDto> Skills { get; }
    
    IQueryable<RecordSkillDto> RecordSkills { get; }
}