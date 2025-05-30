using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Commands.Skill.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateCommand> _validator;
    private readonly ISkillGradeRepository _skillGradeRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateCommand> validator,
        ISkillGradeRepository skillGradeRepository,
        ISkillRepository skillRepository,
        [FromKeyedServices(ModuleKey.Performance)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _skillGradeRepository = skillGradeRepository;
        _skillRepository = skillRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var gradeId = SkillGradeId.Create(command.GradeId).Value;
        
        var grade = await _skillGradeRepository.GetByIdAsync(gradeId, cancellationToken);
        if (grade is null)
        {
            var errorMessage = $"Grade with id {command.GradeId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        var id = SkillId.NewId();
        
        var name = Name.Create(command.Name).Value;
        
        var description = Description.Create(command.Description).Value;
        
        var skill = new Domain.Entities.Skill(id, grade, name, description);
        
        await _skillRepository.AddSkillAsync(skill, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return skill.Id.Value;
    }
}