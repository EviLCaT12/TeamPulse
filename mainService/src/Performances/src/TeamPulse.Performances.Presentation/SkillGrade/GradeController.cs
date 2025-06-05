using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;
using TeamPulse.Performances.Application.Commands.RecordSkill.EmployeeSelfReview;
using TeamPulse.Performances.Application.Commands.SkillGrade;
using TeamPulse.Performances.Application.Commands.SkillGrade.Create;
using TeamPulse.Performances.Contract.Requests.SkillGrade;

namespace TeamPulse.Performances.Presentation.SkillGrade;

public class GradeController : ApplicationController 
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateSkillGrade(
        [FromBody] CreateGradeRequest gradeRequest,
        [FromServices] ICommandHandler<Guid, CreateCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommand(
            gradeRequest.Grades,
            gradeRequest.Name,
            gradeRequest.Description);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpPost("{employeeId:guid}/{groupId:guid}/{skillId:guid}")]
    public async Task<ActionResult> EmployeeSelfReview(
        [FromRoute] Guid employeeId, Guid groupId, Guid skillId, 
        [FromBody] MakeReviewRequest request,
        [FromServices] ICommandHandler<MakeReviewCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new MakeReviewCommand(request.ManagerId, employeeId, request.Grade, groupId, skillId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
}