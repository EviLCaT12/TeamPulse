using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Commands.RecordSkill.EmployeeSelfReview;

public record EmployeeSelfReviewCommand(
    Guid EmployeeId,
    string Grade,
    Guid GroupOfSkillsId,
    Guid SkillId) : ICommand;