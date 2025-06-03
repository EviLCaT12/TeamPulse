namespace TeamPulse.Performances.Contract.Requests.SkillGrade;

public record GetDepartmentManagerGradeForGroupRequest(IEnumerable<Guid> TeamIds, Guid GroupId);