using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.Commands.RecordSkill.EmployeeSelfReview;

public class EmployeeSelfReviewCommandValidator : AbstractValidator<EmployeeSelfReviewCommand>
{
    public EmployeeSelfReviewCommandValidator()
    {
        RuleFor(c => c.Grade).NotEmpty().WithMessage("Grade cannot be empty.");
        RuleFor(c => c.EmployeeId).NotEmpty().WithMessage("Employee cannot be empty.");
        RuleFor(c => c.GroupOfSkillsId).MustBeValueObject(GroupOfSkillsId.Create);
        RuleFor(c => c.SkillId).MustBeValueObject(SkillId.Create);
    }
}