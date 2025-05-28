using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Performances.Application.Commands.GroupOfSkills.AddSkill;
using TeamPulse.Performances.Application.Commands.GroupOfSkills.Create;
using TeamPulse.Performances.Contract.Requests.GroupOfSkill;

namespace TeamPulse.Performances.Presentation.GroupOfSkill;

public class GroupOfSkillController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateGroupOfSkill(
        [FromBody] CreateGroupOfSkillRequest request,
        [FromServices] ICommandHandler<Guid, CreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand(
            request.Name,
            request.Description,
            request.GradeId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("{groupId:guid}/{skillId:guid}")]
    public async Task<ActionResult> AddSkillToGroup(
        [FromRoute] Guid groupId, Guid skillId,
        [FromServices] ICommandHandler<AddSkillToGroupCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddSkillToGroupCommand(groupId, skillId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}