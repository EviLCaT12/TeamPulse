namespace TeamPulse.Performances.Contract.Requests.Skill;

public record CreateSkillRequest(
    Guid GradeId,
    string Name,
    string Description);