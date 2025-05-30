using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Teams.Application.Commands.Team.Create;
using TeamPulse.Teams.Application.Commands.Team.Delete;
using TeamPulse.Teams.Application.Commands.Team.Update;
using TeamPulse.Teams.Application.Queries.Team;
using TeamPulse.Teams.Contract.Dtos;
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

    [HttpPut("{teamId:guid}")]
    public async Task<ActionResult<Guid>> UpdateTeam(
        [FromRoute] Guid teamId,
        [FromBody] UpdateTeamRequest request,
        [FromServices] ICommandHandler<Guid, UpdateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCommand(
            teamId,
            request.NewName,
            request.NewDepartmentId,
            request.NewEmployees,
            request.NewHeadOfTeam);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpDelete("{teamId:guid}")]
    public async Task<ActionResult> DeleteTeam(
        [FromRoute] Guid teamId,
        [FromServices] ICommandHandler<DeleteCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCommand(teamId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }

    [HttpGet("{teamId:guid}")]
    public async Task<ActionResult> GetTeamById(
        [FromRoute] Guid teamId,
        [FromServices] IQueryHandler<TeamDto, GetByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var command = new GetByIdQuery(teamId);
        var result = await handler.HandleAsync(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}