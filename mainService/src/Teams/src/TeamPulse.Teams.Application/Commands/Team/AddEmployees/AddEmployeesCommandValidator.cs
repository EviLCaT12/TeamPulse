using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.AddEmployees;

public class AddEmployeesCommandValidator : AbstractValidator<AddEmployeesCommand>
{
    public AddEmployeesCommandValidator()
    {
        RuleFor(c => c.TeamId).MustBeValueObject(TeamId.Create);
        RuleForEach(c => c.EmployeeIds).MustBeValueObject(EmployeeId.Create);
    }
}