using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Teams.Application.Commands.Team.Create;
using TeamPulse.Teams.Contract.Requests.Team;

namespace TeamPulse.Team.Presentation;

public class TeamController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTeam(
        [FromBody] CreateTeamRequest request,
        [FromServices] ICommandHandler<Guid, CreateTeamCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateTeamCommand(
            request.Name,
            request.DepartmentId,
            request.Employees,
            request.HeadOfTeam);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}