using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.AddTeams;

public class AddTeamsCommandValidator : AbstractValidator<AddTeamsCommand>
{
    public AddTeamsCommandValidator()
    {
        RuleFor(x => x.DepartmentId).MustBeValueObject(DepartmentId.Create);
        RuleForEach(x => x.TeamIds).MustBeValueObject(TeamId.Create);
    }
}   