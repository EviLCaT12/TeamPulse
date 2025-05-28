using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.Delete;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(c => c.TeamId).MustBeValueObject(TeamId.Create);
    }
}