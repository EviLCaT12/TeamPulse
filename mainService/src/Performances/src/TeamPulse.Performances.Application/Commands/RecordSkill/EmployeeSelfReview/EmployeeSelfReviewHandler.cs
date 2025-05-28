using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Contract;

namespace TeamPulse.Performances.Application.Commands.RecordSkill.EmployeeSelfReview;

/// <summary>
/// Самооценивание сотрудником своего конкретного скила
/// </summary>
public class EmployeeSelfReviewHandler : ICommandHandler<EmployeeSelfReviewCommand>
{
    private readonly ILogger<EmployeeSelfReviewHandler> _logger;
    private readonly IValidator<EmployeeSelfReviewCommand> _validator;
    private readonly ITeamContract _teamContract;
    private readonly IRecordSkillRepository _recordSkillRepository;
    private readonly IGroupOfSkillRepository _groupOfSkillRepository;
    private readonly IGroupSkillRepository _groupSkillRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeSelfReviewHandler(
        ILogger<EmployeeSelfReviewHandler> logger,
        IValidator<EmployeeSelfReviewCommand> validator,
        ITeamContract teamContract,
        IRecordSkillRepository recordSkillRepository,
        IGroupOfSkillRepository groupOfSkillRepository,
        IGroupSkillRepository groupSkillRepository,
        [FromKeyedServices(ModuleKey.Performance)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _teamContract = teamContract;
        _recordSkillRepository = recordSkillRepository;
        _groupOfSkillRepository = groupOfSkillRepository;
        _groupSkillRepository = groupSkillRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> HandleAsync(EmployeeSelfReviewCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var groupId = GroupOfSkillsId.Create(command.GroupOfSkillsId).Value;
        
        var skillId = SkillId.Create(command.SkillId).Value;
        
        //Проверим наличие пары группа-скилл
        var getRecordGroupSkillResult = await _groupSkillRepository.GetByIdAsync(
            groupId,
            skillId,
            cancellationToken);
        if (getRecordGroupSkillResult is null)
        {
            var errorMessage = $"There is no pair group {command.GroupOfSkillsId} with skill {command.SkillId}";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        var group = await _groupOfSkillRepository.GetByIdAsync(groupId, cancellationToken);

        var grade = group!.SkillGrade;

        var grades = grade.GradesAsString;
        if (grades.Contains(command.Grade) == false)
        {
            var errorMessage =
                $"Grade {command.Grade} does not exist in the grade list with id {grade.Id.Value}.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }
        
        var getEmployeeResult = await _teamContract.GetEmployeeByIdAsync(command.EmployeeId, cancellationToken);
        if (getEmployeeResult.IsFailure)
            return getEmployeeResult.Error;

        var employee = getEmployeeResult.Value;
        
        var recordSkill = new Domain.Entities.RecordSkill(
            employee.Id,
            skillId.Value,
            command.Grade,
            null);
        
        await _recordSkillRepository.AddRecordSkillAsync(recordSkill, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}