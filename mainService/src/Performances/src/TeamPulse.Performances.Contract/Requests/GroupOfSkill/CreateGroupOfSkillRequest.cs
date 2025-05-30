namespace TeamPulse.Performances.Contract.Requests.GroupOfSkill;

public record CreateGroupOfSkillRequest(
    string Name,
    string Description,
    Guid GradeId);