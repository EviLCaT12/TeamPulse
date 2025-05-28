namespace TeamPulse.Performances.Contract.Requests.SkillGrade;

public record MakeReviewRequest(Guid? ManagerId, string Grade);