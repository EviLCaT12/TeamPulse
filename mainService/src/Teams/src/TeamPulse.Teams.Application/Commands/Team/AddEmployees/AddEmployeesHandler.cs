using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.AddEmployees;

public class AddEmployeesHandler : ICommandHandler<AddEmployeesCommand>
{
    private readonly ILogger<AddEmployeesHandler> _logger;
    private readonly IValidator<AddEmployeesCommand> _validator;
    private readonly ITeamWriteRepository _teamWriteRepository;
    private readonly IEmployeeWriteRepository _employeeWriteRepository;
    private readonly IDepartmentWriteRepository _departmentWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddEmployeesHandler(
        ILogger<AddEmployeesHandler> logger,
        IValidator<AddEmployeesCommand> validator,
        ITeamWriteRepository teamWriteRepository,
        IEmployeeWriteRepository employeeWriteRepository,
        IDepartmentWriteRepository departmentWriteRepository,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _validator = validator;
        _teamWriteRepository = teamWriteRepository;
        _employeeWriteRepository = employeeWriteRepository;
        _departmentWriteRepository = departmentWriteRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(AddEmployeesCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var teamId = TeamId.Create(command.TeamId).Value;
        var team = await _teamWriteRepository.GetTeamByIdAsync(teamId, cancellationToken);
        if (team is null)
        {
            var errorMessage = $"Team with id {teamId} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        //Точно не нул по ограничениям бд
        var departmentId = DepartmentId.Create(team.DepartmentId.Value).Value;
        var department = await _departmentWriteRepository.GetDepartmentByIdAsync(departmentId, cancellationToken: cancellationToken);

        List<Domain.Entities.Employee> employees = [];
        foreach (var id in command.EmployeeIds)
        {
            var employeeId = EmployeeId.Create(id).Value;
            var employee = await _employeeWriteRepository.GetEmployeeByIdAsync(employeeId, cancellationToken);
            if (employee is null)
            {
                var errorMessage = $"Employee with id {employeeId} does not exist.";
                _logger.LogError(errorMessage);
                return Errors.General.ValueNotFound(errorMessage).ToErrorList();
            }
            
            employees.Add(employee);
        }
        
        department!.AddTeamEmployees(teamId, employees);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}