using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Contract;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Performances.Application.Commands.RecordSkill.EmployeeSelfReview;

/// <summary>
/// Самооценивание сотрудником своего конкретного скила
/// </summary>
public class MakeReviewHandler : ICommandHandler<MakeReviewCommand>
{
    private readonly ILogger<MakeReviewHandler> _logger;
    private readonly IValidator<MakeReviewCommand> _validator;
    private readonly ITeamContract _teamContract;
    private readonly IRecordSkillRepository _recordSkillRepository;
    private readonly IGroupOfSkillRepository _groupOfSkillRepository;
    private readonly IGroupSkillRepository _groupSkillRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MakeReviewHandler(
        ILogger<MakeReviewHandler> logger,
        IValidator<MakeReviewCommand> validator,
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

    public async Task<UnitResult<ErrorList>> HandleAsync(MakeReviewCommand command,
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

        Domain.Entities.RecordSkill? recordSkill;
        
        //ToDo: вынести эти проверки в отдельный метод контракта
        if (command.ManagerId is not null)
        {
            var getManagerResult = await _teamContract.GetEmployeeByIdAsync(command.ManagerId.Value, cancellationToken);
            if (getManagerResult.IsFailure)
                return getManagerResult.Error;

            if (getManagerResult.Value.IsTeamManager == false)
            {
                var errorMessage = $"Employee with id {command.ManagerId.Value} is not a manager.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }

            //Руководитель команды может оценивать только своих сотрудников
            if (getManagerResult.Value.TeamId != employee.TeamId)
            {
                var errorMessage =
                    $"Manager {command.ManagerId.Value} and employee {employee.Id} does not belong to one team.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }

            recordSkill = new Domain.Entities.RecordSkill(
                employee.Id,
                skillId.Value,
                null,
                command.Grade);
        }
        
        else
            recordSkill = new Domain.Entities.RecordSkill(
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