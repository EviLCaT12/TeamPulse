using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.AddTeams;

public class AddTeamsHandler : ICommandHandler<AddTeamsCommand>
{
    private readonly ILogger<AddTeamsHandler> _logger;
    private readonly IValidator<AddTeamsCommand> _validator;
    private readonly IDepartmentWriteRepository _departmentWriteRepository;
    private readonly ITeamWriteRepository _teamWriteRepository;
    private readonly IEmployeeWriteRepository _employeeWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddTeamsHandler(
        ILogger<AddTeamsHandler> logger,
        IValidator<AddTeamsCommand> validator,
        IDepartmentWriteRepository departmentWriteRepository,
        ITeamWriteRepository teamWriteRepository,
        IEmployeeWriteRepository employeeWriteRepository,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _departmentWriteRepository = departmentWriteRepository;
        _teamWriteRepository = teamWriteRepository;
        _employeeWriteRepository = employeeWriteRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(AddTeamsCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var departmentId = DepartmentId.Create(command.DepartmentId).Value;
        var department = await _departmentWriteRepository.GetDepartmentByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            var errorMessage = $"Department with id {departmentId.Value} was not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        //Добавляем команды к отделу
        List<Domain.Entities.Team> teamsToAdd = [];
        foreach (var teamId in command.TeamIds)
        {
            var team = await _teamWriteRepository.GetTeamByIdAsync(
                TeamId.Create(teamId).Value,
                cancellationToken);
            
            if (team is null)
            {
                var errorMessage = $"Team with id {teamId} was not found.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }

            var isTeamInDepartment = department.IsTeamInDepartment(team);
            if (isTeamInDepartment)
            {
                var errorMessage = $"Team with id {teamId} is already in department.";
                _logger.LogError(errorMessage);
                Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }
            
            teamsToAdd.Add(team);
        }
        
        department.AddTeams(teamsToAdd);
        
        
        //Добавляем каждого сотрудника команды к отделу
        foreach (var team in teamsToAdd)
        {
            var employees = await _employeeWriteRepository.GetAllEmployeesFromTeamAsync(team.Id, cancellationToken);
            
            department.AddEmployees(employees);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}