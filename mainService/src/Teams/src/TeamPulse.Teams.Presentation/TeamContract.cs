using CSharpFunctionalExtensions;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Application.Commands.Department.Create;
using TeamPulse.Teams.Contract;

namespace TeamPulse.Team.Presentation;

public class TeamContract : ITeamContract
{
    private readonly ICommandHandler<Guid, CreateDepartmentCommand> _handler;

    public TeamContract(ICommandHandler<Guid, CreateDepartmentCommand> handler)
    {
        _handler = handler;
    }

    public async Task<Result<Guid, ErrorList>> CreateDepartmentAsync(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request.Name, request.Teams, request.HeadOfDepartment);
        
        var result = await _handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
        
    }
    
}