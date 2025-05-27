using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Performances.Application.Commands.SkillGrade;
using TeamPulse.Performances.Contract.Requests.SkillGrade;

namespace TeamPulse.Performances.Presentation.SkillGrade;

public class GradeController : ApplicationController 
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateSkillGrade(
        [FromBody] CreateRequest request,
        [FromServices] ICommandHandler<Guid, CreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand(
            request.Grades,
            request.Name,
            request.Description);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}