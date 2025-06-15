using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork,
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<UnitResult<ErrorList>> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var isUserExist = await _userManager.FindByEmailAsync(command.Email);
        if (isUserExist is not null)
        {
            var errorMessage = $"User with email {command.Email} already exists.";
            _logger.LogError(errorMessage);
            return Errors.General.AlreadyExists(errorMessage).ToErrorList();
        }

        var user = new User
        {
            Email = command.Email,
            UserName = command.Name,
        };
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded == false)
        {
            _logger.LogError("User creation failed.");
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();
        
            return new ErrorList(errors);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();

        return UnitResult.Success<ErrorList>();
    }
}