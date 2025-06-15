using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Teams.Application.Commands.Employee.Create;
using TeamPulse.Teams.Application.Queries.Employee;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Team.Presentation;

public class EmployeeController : ApplicationController
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateEmployee(
        [FromServices] ICommandHandler<Guid, CreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpGet("{employeeId:guid}")]
    public async Task<ActionResult> GetEmployeeById(
        [FromRoute] Guid employeeId,
        [FromServices] IQueryHandler<EmployeeDto, GetEmployeeByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var command = new GetEmployeeByIdQuery(employeeId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}