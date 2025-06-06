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

namespace TeamPulse.Teams.Application.Commands.Team.Create;

public class CreateHandler : ICommandHandler<Guid, CreateTeamCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateTeamCommand> _validator;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork,
        IValidator<CreateTeamCommand> validator,
        IDepartmentRepository departmentRepository,
        ITeamRepository teamRepository,
        IEmployeeRepository employeeRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _departmentRepository = departmentRepository;
        _teamRepository = teamRepository;
        _employeeRepository = employeeRepository;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateTeamCommand teamCommand, CancellationToken cancellationToken)
    {
        var transaction = await  _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(teamCommand, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var departmentId = DepartmentId.Create(teamCommand.DepartmentId).Value;
        var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            var errorMessage = $"Department with id {departmentId} was not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        List<Domain.Entities.Employee> employees = [];
        var employeeIds = teamCommand.EmployeeIds ?? [];
        foreach (var employeeId in employeeIds)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(
                EmployeeId.Create(employeeId).Value,
                cancellationToken);
            if (employee is null)
            {
                _logger.LogInformation($"Employee with id {employeeId} was not found.");
                continue;
            }
            employees.Add(employee);
        }
        
        Domain.Entities.Employee? headOfTeam = null;
        if (teamCommand.HeadOfTeamId is not null)
        {
            var employeeId = EmployeeId.Create(teamCommand.HeadOfTeamId!.Value).Value;
            var possibleHeadOfTeam = await _employeeRepository
                .GetEmployeeByIdAsync(employeeId, cancellationToken);
            if (possibleHeadOfTeam is not null)
            {
                if (possibleHeadOfTeam.ManagedTeam is not null)
                {
                    _logger.LogWarning($"Employee with id {employeeId.Value} is already head of team");
                }

                headOfTeam = possibleHeadOfTeam;
            }
            else
            {
                _logger.LogWarning($"Employee with id {employeeId.Value} was not found");
            }
        }

        var teamId = TeamId.CreateNewId();
        
        var name = Name.Create(teamCommand.Name).Value;
        
        var team = new Domain.Entities.Team(teamId, name, department, employees, headOfTeam);
        
        await _teamRepository.AddTeamAsync(team, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return teamId.Value;
    }
}