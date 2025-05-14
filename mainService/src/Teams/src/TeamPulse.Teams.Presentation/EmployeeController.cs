using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Teams.Application.Commands.Employee.Create;

namespace TeamPulse.Team.Presentation;

public class EmployeeController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateEmployee(
        [FromServices] ICommandHandler<Guid, CreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand();
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}