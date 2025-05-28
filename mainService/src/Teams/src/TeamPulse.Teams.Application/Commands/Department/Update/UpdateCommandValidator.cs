using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(c => c.DepartmentId).MustBeValueObject(DepartmentId.Create);
        RuleFor(c => c.NewName).MustBeValueObject(Name.Create);
        RuleForEach(c => c.NewTeams).MustBeValueObject(TeamId.Create);
        RuleFor(c => c.NewHeadOfDepartment)
            .NotEqual(Guid.Empty)
            .When(c => c.NewHeadOfDepartment != null);
    }
}