using CSharpFunctionalExtensions;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.Commands.Department.Create;
using TeamPulse.Teams.Application.Commands.Team.Create;
using TeamPulse.Teams.Contract;
using TeamPulse.Teams.Contract.Requests.Department;
using TeamPulse.Teams.Contract.Requests.Team;

namespace TeamPulse.Team.Presentation;

public class TeamContract : ITeamContract
{
    private readonly ICommandHandler<Guid, CreateDepartmentCommand> _createDepartmentHandler;
    private readonly ICommandHandler<Guid, CreateTeamCommand> _createTeamCommandHandler;

    public TeamContract(
        ICommandHandler<Guid, CreateDepartmentCommand> createDepartmentHandler,
        ICommandHandler<Guid, CreateTeamCommand> createTeamCommandHandler)
    {
        _createDepartmentHandler = createDepartmentHandler;
        _createTeamCommandHandler = createTeamCommandHandler;
    }

    public async Task<Result<Guid, ErrorList>> CreateDepartmentAsync(CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request.Name, request.Teams, request.HeadOfDepartment);

        var result = await _createDepartmentHandler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }

    public async Task<Result<Guid, ErrorList>> CreateTeamAsync(CreateTeamRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTeamCommand(
            request.Name,
            request.DepartmentId,
            request.Employees,
            request.HeadOfTeam);
        
        var result = await _createTeamCommandHandler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }
}