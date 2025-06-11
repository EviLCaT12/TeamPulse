using AuthService.Domain;
using AuthService.Errors;
using AuthService.Extensions;
using AuthService.Jwt;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Features;

public record LoginRequest(string Email, string Password);


public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Password).NotEmpty();
    }
}


[Route("login")]
public sealed class LoginController : ApplicationController
{
    private readonly LoginHandler _handler;

    public LoginController(LoginHandler handler)
    {
        _handler = handler;
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _handler.HandleAsync(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }
}


public sealed class LoginHandler
{
    private readonly IValidator<LoginRequest> _validator;
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginHandler(
        IValidator<LoginRequest> validator, 
        UserManager<User> userManager,
        ITokenGenerator tokenGenerator)
    {
        _validator = validator;
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<string, ErrorList>> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Errors.Errors.General
                .ValueNotFound($"User with email address {request.Email}  does not exist.")
                .ToErrorList();
        
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, request.Password);
        if (checkPasswordResult == false)
            return Errors.Errors.General.ValueIsInvalid("Password is incorrect.").ToErrorList();

        var jwtGenerateResult = _tokenGenerator.GenerateToken(user);
        if (jwtGenerateResult.IsFailure)
            return jwtGenerateResult.Error.ToErrorList();
        
        return jwtGenerateResult.Value;
    }
}