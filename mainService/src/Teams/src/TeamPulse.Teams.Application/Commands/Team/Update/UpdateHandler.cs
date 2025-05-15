using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.Update;

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

        var teamId = TeamId.Create(command.TeamId).Value;
        var team = await _teamRepository
            .GetTeamIdAsync(teamId, cancellationToken);
        if (team is null)
        {
            var errorMessage = $"Team with id {teamId.Value} not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }
        
        //ToDO возможно стоит проверять принадлежность команды к отделу (изменять команды может руководитель отдела)
        var department = await _departmentRepository.GetDepartmentByIdAsync(team.Department.Id, cancellationToken);
        //По задумке такая ситуация невозможна, но на всякий случай проверю
        if (department is null)
        {
            var errorMessage = $"Department with id {team.Department.Id.Value} not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        if (command.NewName is not null)
        {
            var newName = Name.Create(command.NewName).Value;
            var updateNameResult = department.UpdateTeamName(teamId, newName);
            if (updateNameResult.IsFailure)
                return updateNameResult.Error.ToErrorList();
        }

        if (command.NewEmployees is not null)
        {
            List<Domain.Entities.Employee> employees = [];
            foreach (var employee in command.NewEmployees)
            {
                var employeeId = EmployeeId.Create(employee).Value;
                var newEmployee = await _employeeRepository.GetEmployeeByIdAsync(employeeId, cancellationToken);
                if (newEmployee is null)
                {
                    var errorMessage = $"Employee with id {employeeId.Value} not found.";
                    _logger.LogError(errorMessage);
                    return Errors.General.ValueNotFound(errorMessage).ToErrorList();
                }
                
                employees.Add(newEmployee);
            }
            
            var updateResult = department.UpdateTeamEmployees(teamId, employees);
            if (updateResult.IsFailure)
                return updateResult.Error.ToErrorList();
        }

        if (command.NewHeadOfTeam is not null)
        {
            var employeeId = EmployeeId.Create(command.NewHeadOfTeam.Value).Value;
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId, cancellationToken);
            if (employee is not null)
            {
                if (employee.ManagedTeam is not null)
                {
                    var errorMessage = $"Employee with id {employeeId.Value} has already managed team with id {employee.ManagedTeam.Id.Value}.";
                    _logger.LogError(errorMessage);
                    return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
                }

                var updateResult = department.UpdateHeadOfTeam(teamId, employee);
                if (updateResult.IsFailure)
                    return updateResult.Error.ToErrorList();
            }
            else
            {
                var errorMessage = $"Employee with id {employeeId.Value} not found.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }
        }

        if (command.NewDepartmentId is not null)
        {
            var newDepartmentId = DepartmentId.Create(command.NewDepartmentId.Value).Value;
            var newDepartment = await _departmentRepository.GetDepartmentByIdAsync(newDepartmentId, cancellationToken);
            if (newDepartment is null)
            {
                var errorMessage = $"Department with id {team.Department.Id.Value} not found.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }
            
            department.RemoveTeam(team);
            newDepartment.AddTeams([team]);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return teamId.Value;
    }
}