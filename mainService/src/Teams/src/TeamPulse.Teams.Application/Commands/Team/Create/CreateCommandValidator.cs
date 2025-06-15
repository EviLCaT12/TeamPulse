using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.Create;

public class CreateCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
        RuleFor(c => c.DepartmentId).MustBeValueObject(DepartmentId.Create);
        RuleFor(c => c.HeadOfTeamId).MustBeValueObject(EmployeeId.Create);
    }
}