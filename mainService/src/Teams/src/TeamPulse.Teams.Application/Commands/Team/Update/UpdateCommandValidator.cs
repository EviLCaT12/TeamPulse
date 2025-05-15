using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(c => c.TeamId).MustBeValueObject(TeamId.Create);
        RuleFor(c => c.NewName).MustBeValueObject(Name.Create);
        RuleFor(c => c.NewDepartmentId)
            .MustBeValueObject(id => DepartmentId.Create(id!.Value))
            .When(c => c.NewDepartmentId is not null);
        RuleForEach(c => c.NewEmployees).MustBeValueObject(EmployeeId.Create);
        RuleFor(c => c.NewHeadOfTeam)
            .MustBeValueObject(id => EmployeeId.Create(id!.Value))
            .When(c => c.NewEmployees is not null);
    }
}