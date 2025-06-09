using AuthService.Domain;
using AuthService.EmailSender;
using AuthService.Errors;
using AuthService.Extensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Features;

public record RegisterUserRequest(string Email, string Password);


public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).NotEmpty();
    }
}


[Route("/registration")]
public sealed class RegisterUserController : ApplicationController
{
    private readonly RegisterUserHandler _handler;

    public RegisterUserController(RegisterUserHandler handler)
    {
        _handler = handler;
    }
    
    [HttpPost]
    public async Task<ActionResult<Guid>> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}


public sealed class RegisterUserHandler
{
    private readonly UserManager<User> _userManager;
    private readonly IValidator<RegisterUserRequest> _validator;
    private readonly IMailSender _mailSender;

    public RegisterUserHandler(
        UserManager<User> userManager,
        IValidator<RegisterUserRequest> validator,
        IMailSender mailSender)
    {
        _userManager = userManager;
        _validator = validator;
        _mailSender = mailSender;
    }
    public async Task<Result<Guid, ErrorList>> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded == false)
        {
            var errors = result.Errors.Select(e => Errors.Errors.General.ValueIsInvalid(e.Description));
            return new ErrorList(errors);
        }

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var confirmationLink = 
            "http://localhost:5000/email-verification" + 
            "?userId=" + user.Id +
            "&token=" + Base64UrlEncoder.Encode(confirmationToken);
        
        await _mailSender.SendMailAsync(new MailData(
            user.Email,
            "Подтверждение регистрации",
            $"Для подтверждения регистрации перейдите по ссылке: {confirmationLink}"));
        
        return user.Id;
    }
}