using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Application.Commands.Login;

public class LoginHandler : ICommandHandler<string, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }
    public async Task<Result<string, ErrorList>> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            var errorMessage = $"User with email {command.Email} does not exist.";
            _logger.LogError(errorMessage);
            return Errors.General.ValueNotFound(errorMessage).ToErrorList();
        }
        
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, command.Password);
        if (checkPasswordResult == false)
            return Errors.General.ValueIsInvalid("Invalid credentials.").ToErrorList();

        var token =  await _tokenProvider.GenerateTokenAsync(user, cancellationToken);

        return token;
    }
}