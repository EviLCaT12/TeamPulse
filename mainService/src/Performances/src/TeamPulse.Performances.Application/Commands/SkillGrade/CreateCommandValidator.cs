using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Commands.SkillGrade;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.Grades).NotEmpty();
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}