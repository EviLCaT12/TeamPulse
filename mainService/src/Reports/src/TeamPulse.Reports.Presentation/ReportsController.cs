using Microsoft.AspNetCore.Mvc;
using TeamPulse.Core.Abstractions;
using TeamPulse.Framework;
using TeamPulse.Framework.Authorization;
using TeamPulse.Framework.Responses;
using TeamPulse.Reports.Application.Commands.GenerateMedianValueReport;
using TeamPulse.Reports.Contract.Requests;
using TeamPulse.Reports.Domain.Enums;
using TeamPulse.Reports.Domain.Reports;

namespace TeamPulse.Reports.Presentation;

public class ReportsController : ApplicationController
{
    [Permission(Permissions.Reports.GetMedianValueReports)]
    [HttpPost("{objectId:guid}/{subjectId:guid}")]
    public async Task<ActionResult<BaseReport>> GetMedianValueReports(
        [FromRoute] Guid objectId, Guid subjectId,
        [FromBody] GetMedianValueReportsRequest request,
        [FromServices] ICommandHandler<BaseReport, GenerateMedianValueCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new GenerateMedianValueCommand(
            request.Name,
            request.Description,
            objectId,
            (ObjectType)request.ObjectType,
            subjectId);
        
        var result = await handler.HandleAsync(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}