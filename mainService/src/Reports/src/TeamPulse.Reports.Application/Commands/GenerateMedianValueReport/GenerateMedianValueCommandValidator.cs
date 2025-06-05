using FluentValidation;
using TeamPulse.Core.Validators;
using TeamPulse.Reports.Domain.Enums;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Application.Commands.GenerateMedianValueReport;

public class GenerateMedianValueCommandValidator : AbstractValidator<GenerateMedianValueCommand>
{
    public GenerateMedianValueCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Object)
            .NotEmpty()
            .WithMessage("Context object cannot be null or empty.");
        RuleFor(c => c.Subject)
            .NotEmpty()
            .WithMessage("Context subject cannot be null or empty.");
    }
} 