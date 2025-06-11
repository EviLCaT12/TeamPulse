using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Name).MustBeValueObject(Name.Create);
        RuleFor(x => x.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.GradeId).MustBeValueObject(SkillGradeId.Create);
        RuleFor(c => c.DepartmentId)
            .NotEmpty()
            .WithMessage("A department must be specified");
        RuleFor(c => c.HeadOfDepartmentId)
            .NotEmpty()
            .WithMessage("Head of department must be specified");
    }
}