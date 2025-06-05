using TeamPulse.Core.Abstractions;

namespace TeamPulse.Performances.Application.Commands.RecordSkill.EmployeeSelfReview;

public record MakeReviewCommand(
    Guid? ManagerId,
    Guid EmployeeId,
    string Grade,
    Guid GroupOfSkillsId,
    Guid SkillId) : ICommand;