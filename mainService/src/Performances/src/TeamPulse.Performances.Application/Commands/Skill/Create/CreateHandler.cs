using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Commands.Skill.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateCommand> _validator;
    private readonly ISkillGradeWriteRepository _skillGradeWriteRepository;
    private readonly ISkillWriteRepository _skillWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateCommand> validator,
        ISkillGradeWriteRepository skillGradeWriteRepository,
        ISkillWriteRepository skillWriteRepository,
        [FromKeyedServices(ModuleKey.Performance)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _skillGradeWriteRepository = skillGradeWriteRepository;
        _skillWriteRepository = skillWriteRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var gradeId = SkillGradeId.Create(command.GradeId).Value;
        
        var grade = await _skillGradeWriteRepository.GetByIdAsync(gradeId, cancellationToken);
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
        
        await _skillWriteRepository.AddSkillAsync(skill, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return skill.Id.Value;
    }
}