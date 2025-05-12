using Microsoft.AspNetCore.Mvc;
using TeamPulse.Framework.Responses;

namespace TeamPulse.Framework;

[ApiController]
public class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return base.Ok(envelope);
    }
}