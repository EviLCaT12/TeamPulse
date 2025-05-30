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

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateCommand> _validator;
    private readonly ISkillGradeRepository _gradesRepository;
    private readonly IGroupOfSkillRepository _groupsOfSkillRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateCommand> validator,
        ISkillGradeRepository gradesRepository,
        IGroupOfSkillRepository groupsOfSkillRepository,
        [FromKeyedServices(ModuleKey.Performance)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _gradesRepository = gradesRepository;
        _groupsOfSkillRepository = groupsOfSkillRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var gradeId = SkillGradeId.Create(command.GradeId).Value;

        var grade = await _gradesRepository.GetByIdAsync(gradeId, cancellationToken);
        if (grade is null)
        {
            var errorMessage = $"Grade with id {command.GradeId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        var groupId = GroupOfSkillsId.NewId();
        
        var name = Name.Create(command.Name).Value;
        
        var description = Description.Create(command.Description).Value;
        
        var group = new Domain.Entities.GroupOfSkills(groupId, name, description, grade);
        
        await _groupsOfSkillRepository.AddAsync(group, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return group.Id.Value;
    }
}