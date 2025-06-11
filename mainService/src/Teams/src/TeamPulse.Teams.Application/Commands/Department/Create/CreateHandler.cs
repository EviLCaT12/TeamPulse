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

namespace TeamPulse.Teams.Application.Commands.Department.Create;

public class CreateHandler : ICommandHandler<Guid, CreateDepartmentCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IValidator<CreateDepartmentCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDepartmentWriteRepository _departmentWriteRepository;
    private readonly ITeamWriteRepository _teamWriteRepository;
    private readonly IEmployeeWriteRepository _employeeWriteRepository;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        IValidator<CreateDepartmentCommand> validator,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork,
        IDepartmentWriteRepository departmentWriteRepository,
        ITeamWriteRepository teamWriteRepository,
        IEmployeeWriteRepository employeeWriteRepository)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _departmentWriteRepository = departmentWriteRepository;
        _teamWriteRepository = teamWriteRepository;
        _employeeWriteRepository = employeeWriteRepository;
    }

    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateDepartmentCommand departmentCommand,
        CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var validationResult = await _validator.ValidateAsync(departmentCommand, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        //Тут можно сразу брать value, так как команда уже прошла валидацию
        var departmentId = DepartmentId.CreateNewId();

        var name = Name.Create(departmentCommand.Name).Value;

        var teams = departmentCommand.Teams?.ToList() ?? [];
        List<Domain.Entities.Team> departmentTeams = [];
        if (teams.Count != 0)
        {
            foreach (var team in teams)
            {
                var teamId = TeamId.Create(team).Value;
                var departmentTeam = await _teamWriteRepository.GetTeamByIdAsync(teamId, cancellationToken);
                if (departmentTeam is null)
                {
                    _logger.LogWarning($"Team with id {teamId.Value} was not found");
                    continue;
                }

                departmentTeams.Add(departmentTeam);
            }
        }

        Domain.Entities.Employee? employee;
        var employeeId = EmployeeId.Create(departmentCommand.HeadOfDepartment).Value;
        var headOfDepartment = await _employeeWriteRepository
            .GetEmployeeByIdAsync(employeeId, cancellationToken);
        if (headOfDepartment is not null)
        {
            if (headOfDepartment.IsDepartmentManager)
            {
                var errorMessage = $"Employee with id {employeeId.Value} is already head of department";
                _logger.LogWarning(errorMessage);
                return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }

            employee = headOfDepartment;
        }
        else
        {
            var errorMessage = $"Employee with id {employeeId.Value} was not found";
            _logger.LogWarning(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        var department = new Domain.Entities.Department(
            departmentId,
            name,
            departmentTeams,
            employee);

        await _departmentWriteRepository.AddDepartmentAsync(department, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        return department.Id.Value;
    }
}