using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.Update;

public class UpdateHandler : ICommandHandler<Guid, UpdateCommand>
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IValidator<UpdateCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateHandler(
        ILogger<UpdateHandler> logger,
        IValidator<UpdateCommand> validator,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork,
        IDepartmentRepository departmentRepository,
        ITeamRepository teamRepository,
        IEmployeeRepository employeeRepository)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _departmentRepository = departmentRepository;
        _teamRepository = teamRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(UpdateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var departmentId = DepartmentId.Create(command.DepartmentId).Value;
        var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            var errorMessage = $"Department with id {departmentId.Value} was not found";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }
        
        /*
         * Если прислали такие же значения, значит ничего не меняем
         * Если null, то кидаем ошибку
         * Если новое, то сохраняем
         */
        if (command.NewName is not null && department.Name.Value != command.NewName)
        {
            var newName = Name.Create(command.NewName).Value;
            department.UpdateName(newName);
        }
        
        var teams = command.NewTeams?.ToList() ?? [];
        List<Domain.Entities.Team> newDepartmentTeams = [];
        if (teams.Count != 0)
        {
            foreach (var team in teams)
            {
                var teamId = TeamId.Create(team).Value;
                var departmentTeam = await _teamRepository.GetTeamIdAsync(teamId, cancellationToken);
                if (departmentTeam is null)
                {
                    var errorMessage = $"Team with id {teamId.Value} was not found";
                    _logger.LogError(errorMessage);
                    return Errors.General.ValueNotFound(errorMessage).ToErrorList();
                }

                newDepartmentTeams.Add(departmentTeam);
            }
        }

        if (department.Teams.SequenceEqual(newDepartmentTeams) == false)
        {
            department.UpdateTeams(newDepartmentTeams);
        }


        Domain.Entities.Employee? employee = null;
        if (command.NewHeadOfDepartment is not null)
        {
            var employeeId = EmployeeId.Create(command.NewHeadOfDepartment!.Value).Value;
            var headOfDepartment = await _employeeRepository
                .GetEmployeeByIdAsync(employeeId, cancellationToken);
            if (headOfDepartment is not null)
            {
                if (headOfDepartment.ManagedDepartmentId is not null)
                {
                    var errorMessage = $"Employee with id {employeeId.Value} is already head of department";
                    _logger.LogError(errorMessage);
                    return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
                }

                employee = headOfDepartment;
            }
            else
            {
                var errorMessage = $"Employee with id {employeeId.Value} was not found";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }
        }

        if (department.HeadOfDepartment != employee && employee is not null)
        {
            department.UpdateHeadOfDepartment(employee);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        return departmentId.Value;
    }
}