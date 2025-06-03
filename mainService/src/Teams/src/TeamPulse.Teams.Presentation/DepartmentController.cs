using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Teams.Application.Commands.Department.AddTeamsToDepartment;
using TeamPulse.Teams.Application.Commands.Department.Create;
using TeamPulse.Teams.Application.Commands.Department.Delete;
using TeamPulse.Teams.Application.Commands.Department.Update;
using TeamPulse.Teams.Application.Queries.Department;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Contract.Requests.Department;

namespace TeamPulse.Team.Presentation;

public class DepartmentController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateDepartment(
        [FromBody] CreateDepartmentRequest request,
        [FromServices] ICommandHandler<Guid, CreateDepartmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(
            request.Name,
            request.Teams,
            request.HeadOfDepartment);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPut("{departmentId:guid}")]
    public async Task<ActionResult<Guid>> UpdateDepartment(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateDepartmentRequest request,
        [FromServices] ICommandHandler<Guid, UpdateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCommand(
            departmentId,
            request.NewName,
            request.NewTeams,
            request.NewHeadOfDepartment);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpDelete("{departmentId:guid}")]
    public async Task<ActionResult> DeleteDepartment(
        [FromRoute] Guid departmentId,
        [FromServices] ICommandHandler<DeleteCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCommand(departmentId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet("{departmentId:guid}")]
    public async Task<ActionResult> GetDepartmentById(
        [FromRoute] Guid departmentId,
        [FromServices] IQueryHandler<DepartmentDto, GetDepartmentByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetDepartmentByIdQuery(departmentId);
        
        var result = await handler.HandleAsync(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("{departmentId:guid}/add-teams")]
    public async Task<ActionResult> AddTeamsToDepartment(
        [FromRoute] Guid departmentId,
        [FromBody] AddTeamsToDepartmentRequest request,
        [FromServices] ICommandHandler<AddTeamsToDepartmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddTeamsToDepartmentCommand(departmentId, request.TeamIds.ToList());
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
}