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
        RuleForEach(c => c.EmployeeIds).MustBeValueObject(EmployeeId.Create);
        RuleFor(c => c.HeadOfTeamId)
            .NotEqual(Guid.Empty)
            .When(c => c.HeadOfTeamId is not null);
    }
}