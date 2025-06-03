using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.AddTeamsToDepartment;

public class AddTeamsToDepartmentCommandValidator : AbstractValidator<AddTeamsToDepartmentCommand>
{
    public AddTeamsToDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId).MustBeValueObject(DepartmentId.Create);
        RuleForEach(x => x.TeamIds).MustBeValueObject(TeamId.Create);
    }
}