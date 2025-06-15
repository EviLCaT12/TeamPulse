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
using TeamPulse.Teams.Contract;

namespace TeamPulse.Performances.Application.Commands.GroupOfSkills.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateCommand> _validator;
    private readonly ISkillGradeWriteRepository _gradesWriteRepository;
    private readonly IGroupOfSkillWriteRepository _groupsOfSkillWriteRepository;
    private readonly IRecordSkillWriteRepository _recordSkillWriteRepository;
    private readonly ITeamContract _teamContract;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateCommand> validator,
        ISkillGradeWriteRepository gradesWriteRepository,
        IGroupOfSkillWriteRepository groupsOfSkillWriteRepository,
        IRecordSkillWriteRepository recordSkillWriteRepository,
        ITeamContract teamContract,
        [FromKeyedServices(ModuleKey.Performance)]
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _gradesWriteRepository = gradesWriteRepository;
        _groupsOfSkillWriteRepository = groupsOfSkillWriteRepository;
        _recordSkillWriteRepository = recordSkillWriteRepository;
        _teamContract = teamContract;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var isDepartmentExist = await _teamContract
            .GetDepartmentByIdAsync(command.DepartmentId, cancellationToken);
        if (isDepartmentExist.IsFailure)
            return isDepartmentExist.Error;
        var department = isDepartmentExist.Value;

        var headOfDepartment = department.HeadOfDepartment;
        if (headOfDepartment is null || headOfDepartment.Id != command.HeadOfDepartmentId)
        {
            var errorMessage = $"Invalid Head of Department: {command.HeadOfDepartmentId}";
            _logger.LogError(errorMessage);
            Errors.General.ValueIsInvalid(errorMessage);
        }

        var gradeId = SkillGradeId.Create(command.GradeId).Value;

        var grade = await _gradesWriteRepository.GetByIdAsync(gradeId, cancellationToken);
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

        await _groupsOfSkillWriteRepository.AddAsync(group, cancellationToken);

        await _recordSkillWriteRepository.AddRecordSkillAsync(
            new Domain.Entities.RecordSkill(department.Id, groupId.Value, null, null),
            cancellationToken);

        //Каждая команда из отдела подтягивает себе созданную группу
        var getTeamsFromDepartment =
            await _teamContract.GetAllTeamsFromDepartmentAsync(department.Id, cancellationToken);

        if (getTeamsFromDepartment.IsFailure)
            return getTeamsFromDepartment.Error;

        var teams = getTeamsFromDepartment.Value;

        foreach (var team in teams)
        {
            //Каждому сотруднику команды также требуется сделать эту запись 
            var getEmployeesFromTeam = await _teamContract.GetAllEmployeesFromTeamAsync(team, cancellationToken);

            if (getEmployeesFromTeam.IsFailure)
                return getEmployeesFromTeam.Error;

            var employees = getEmployeesFromTeam.Value;

            foreach (var employee in employees)
            {
                var employeeRecordSkill = new Domain.Entities.RecordSkill(
                    employee,
                    groupId.Value,
                    null,
                    null);
                await _recordSkillWriteRepository.AddRecordSkillAsync(employeeRecordSkill, cancellationToken);
            }

            var teamRecordSkill = new Domain.Entities.RecordSkill(
                team,
                groupId.Value,
                null,
                null);
            await _recordSkillWriteRepository.AddRecordSkillAsync(teamRecordSkill, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        return group.Id.Value;
    }
}