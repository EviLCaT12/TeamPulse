using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Authorization;
using TeamPulse.Framework.Responses;
using TeamPulse.Performances.Application.Commands.Skill.Create;
using TeamPulse.Performances.Contract.Requests.Skill;

namespace TeamPulse.Performances.Presentation.SKill;

public class SkillController : ApplicationController
{
    [Permission(Permissions.Performances.CreateSkill)]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateSkill(
        [FromBody] CreateSkillRequest skillRequest,
        [FromServices] ICommandHandler<Guid, CreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand(
            skillRequest.GradeId,
            skillRequest.Name,
            skillRequest.Description);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}