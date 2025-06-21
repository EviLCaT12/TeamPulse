using Microsoft.AspNetCore.Mvc;
using TeamPulse.Accounts.Application.Commands.Login;
using TeamPulse.Accounts.Application.Commands.RefreshTokens;
using TeamPulse.Accounts.Application.Commands.RegisterUser;
using TeamPulse.Accounts.Contracts.Requests;
using TeamPulse.Framework;
using TeamPulse.Framework.Responses;

namespace TeamPulse.Accounts.Presentation;

public class AccountController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Name,
            request.Password);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Login(
        [FromBody] RefreshTokenRequest request,
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokensCommand(
            request.AccessToken,
            request.RefreshToken);
        
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
    
}