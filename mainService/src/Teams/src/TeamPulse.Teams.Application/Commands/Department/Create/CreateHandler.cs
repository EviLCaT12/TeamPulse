using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Department.Create;

public class CreateHandler : ICommandHandler<Guid, CreateCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateCommand> validator,
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

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        //Тут можно сразу брать value, так как команда уже прошла валидацию
        var departmentId = DepartmentId.CreateNewId();

        var name = Name.Create(command.Name).Value;

        var teams = command.Teams?.ToList() ?? [];
        List<Team> departmentTeams = [];
        if (teams.Count != 0)
        {
            foreach (var team in teams)
            {
                var departmentTeam = await _teamRepository.GetTeamIdAsync(team, cancellationToken);
                if (departmentTeam is null)
                {
                    _logger.LogWarning($"Team with id {team} was not found");
                    continue;
                }

                departmentTeams.Add(departmentTeam);
            }
        }

        Employee? employee = null;
        if (command.HeadOfDepartment is not null)
        {
            var headOfDepartment = await _employeeRepository
                .GetEmployeeByIdAsync(command.HeadOfDepartment, cancellationToken);
            if (headOfDepartment is not null)
            {
                if (headOfDepartment.Department is not null)
                {
                    _logger.LogWarning($"Employee with id {headOfDepartment.Id} is already head of department");
                }

                employee = headOfDepartment;
            }
            else
            {
                _logger.LogWarning($"Employee with id {command.HeadOfDepartment} was not found");
            }
        }

        var department = new Domain.Entities.Department(
            departmentId,
            name,
            departmentTeams,
            employee);
        
        await _departmentRepository.AddDepartmentAsync(department, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
        
        return department.Id.Value;
    }
}