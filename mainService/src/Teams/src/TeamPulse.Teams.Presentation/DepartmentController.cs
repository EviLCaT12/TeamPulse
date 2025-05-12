using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Teams.Application.Commands.Department.Create;
using TeamPulse.Teams.Contract;

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
}