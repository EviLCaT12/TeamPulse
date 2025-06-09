using AuthService.Domain;
using AuthService.Errors;
using AuthService.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit.Encodings;

namespace AuthService.Features;

public record VerifyEmailRequest(Guid UserId, string Token);

[Route("email-verification")]
public sealed class VerifyEmailController : ApplicationController
{
    private readonly VerifyEmailHandler _handler;

    public VerifyEmailController(VerifyEmailHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<Guid>> VerifyEmail([FromQuery] VerifyEmailRequest request, CancellationToken ct)
    {
        var result = await _handler.HandlerAsync(request, ct);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}

public sealed class VerifyEmailHandler
{
    private readonly UserManager<User> _userManager;

    public VerifyEmailHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Guid, ErrorList>> HandlerAsync(VerifyEmailRequest request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return Errors.Errors.General
                .ValueNotFound($"User with id {request.UserId} does not exist")
                .ToErrorList();
        
        var result = await _userManager.ConfirmEmailAsync(user, Base64UrlEncoder.Decode(request.Token));
        if (result.Succeeded == false)
        {
            var errors = result.Errors
                .Select(e => Errors.Errors.General.ValueIsInvalid(e.Description));
            
            return new ErrorList(errors);
        }
        
        return user.Id;
    }
}