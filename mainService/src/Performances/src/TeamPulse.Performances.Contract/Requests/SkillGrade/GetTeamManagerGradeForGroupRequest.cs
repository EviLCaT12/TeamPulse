namespace TeamPulse.Performances.Contract.Requests.SkillGrade;

public record GetTeamManagerGradeForGroupRequest(List<Guid> EmployeeIds, Guid GroupId);