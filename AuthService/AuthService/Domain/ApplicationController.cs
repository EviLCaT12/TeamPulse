using AuthService.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Domain;

[ApiController]
[Route("api/auth")]
public class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope.Ok(value);
        
        return base.Ok(envelope);
    }
}