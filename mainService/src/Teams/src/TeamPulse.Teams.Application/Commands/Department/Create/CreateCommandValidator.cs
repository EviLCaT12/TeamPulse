using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
        RuleForEach(c => c.Teams).MustBeValueObject(TeamId.Create);
        RuleFor(c => c.HeadOfDepartment).MustBeValueObject(EmployeeId.Create);
    }
}