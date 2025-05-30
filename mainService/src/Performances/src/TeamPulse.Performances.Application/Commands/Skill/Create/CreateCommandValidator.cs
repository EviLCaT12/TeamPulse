using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Commands.Skill.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.GradeId).MustBeValueObject(SkillGradeId.Create);
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    }
}