using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.Factories;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Commands.SkillGrade.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateCommand> _validator;
    private readonly ISkillGradeRepository _skillGradeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateCommand> validator,
        ISkillGradeRepository skillGradeRepository,
        [FromKeyedServices(ModuleKey.Performance)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _skillGradeRepository = skillGradeRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var name = Name.Create(command.Name).Value;
        
        var description = Description.Create(command.Description).Value;
        
        var skillGrade = SkillGradeFactory.CreateSkillGrade(
            command.Grades,
            name,
            description);
        
        if (skillGrade.IsFailure)
            return skillGrade.Error.ToErrorList();
        
        await _skillGradeRepository.AddAsync(skillGrade.Value, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
        
        return skillGrade.Value.Id.Value;
    }
}