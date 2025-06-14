using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Core.Abstractions;
using TeamPulse.Core.Validators;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Application.Commands.Team.Create;

public class CreateHandler : ICommandHandler<Guid, CreateTeamCommand>
{
    private readonly ILogger<CreateHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateTeamCommand> _validator;
    private readonly IDepartmentWriteRepository _departmentWriteRepository;
    private readonly ITeamWriteRepository _teamWriteRepository;
    private readonly IEmployeeWriteRepository _employeeWriteRepository;

    public CreateHandler(
        ILogger<CreateHandler> logger,
        [FromKeyedServices(ModuleKey.Team)] IUnitOfWork unitOfWork,
        IValidator<CreateTeamCommand> validator,
        IDepartmentWriteRepository departmentWriteRepository,
        ITeamWriteRepository teamWriteRepository,
        IEmployeeWriteRepository employeeWriteRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _departmentWriteRepository = departmentWriteRepository;
        _teamWriteRepository = teamWriteRepository;
        _employeeWriteRepository = employeeWriteRepository;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(CreateTeamCommand teamCommand, CancellationToken cancellationToken)
    {
        var transaction = await  _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var validationResult = await _validator.ValidateAsync(teamCommand, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var departmentId = DepartmentId.Create(teamCommand.DepartmentId).Value;
        var department = await _departmentWriteRepository.GetDepartmentByIdAsync(departmentId, cancellationToken);
        if (department is null)
        {
            var errorMessage = $"Department with id {departmentId} was not found.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }
        
        Domain.Entities.Employee headOfTeam;
        
        var employeeId = EmployeeId.Create(teamCommand.HeadOfTeamId).Value;
        var possibleHeadOfTeam = await _employeeWriteRepository
            .GetEmployeeByIdAsync(employeeId, cancellationToken);
        if (possibleHeadOfTeam is not null)
        {
            if (possibleHeadOfTeam.IsTeamManager)
            {
                var errorMessage = $"Employee with id {employeeId.Value} is already head of team";
                _logger.LogWarning(errorMessage);
                return Errors.General.ValueIsInvalid(errorMessage).ToErrorList();
            }

            headOfTeam = possibleHeadOfTeam;
        }
        else
        {
            var errorMessage = $"Employee with id {employeeId.Value} was not found.";
            _logger.LogWarning(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }

        var teamId = TeamId.CreateNewId();
        
        var name = Name.Create(teamCommand.Name).Value;
        
        var team = new Domain.Entities.Team(teamId, name, departmentId, headOfTeam);
        
        await _teamWriteRepository.AddTeamAsync(team, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return teamId.Value;
    }
}