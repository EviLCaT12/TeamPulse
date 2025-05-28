using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.AddSkill;

public class AddSkillToGroupCommandValidator : AbstractValidator<AddSkillToGroupCommand>
{
    public AddSkillToGroupCommandValidator()
    {
        RuleFor(c => c.GroupId).MustBeValueObject(GroupOfSkillsId.Create);
        RuleFor(c => c.SkillId).MustBeValueObject(SkillId.Create);
    }
}