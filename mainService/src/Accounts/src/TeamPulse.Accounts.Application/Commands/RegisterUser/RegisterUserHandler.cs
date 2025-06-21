using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamPulse.Accounts.Application.AccountManagers;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Domain.Models.AccountModels;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.Teams.Contract;

namespace TeamPulse.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly ITeamContract _teamContract;

    public RegisterUserHandler(
        IAccountManager accountManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        [FromKeyedServices(ModuleKey.Account)] IUnitOfWork unitOfWork,
        ILogger<RegisterUserHandler> logger,
        ITeamContract teamContract)
    {
        _accountManager = accountManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _teamContract = teamContract;
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

        var role = await _roleManager.FindByNameAsync(EmployeeAccount.Employee);

        var employeeUser = User.CreateEmployee(
            command.Name,
            command.Email,
            role!).Value;
        
        var result = await _userManager.CreateAsync(employeeUser, command.Password);
        if (result.Succeeded == false)
        {
            _logger.LogError("User creation failed.");
            var errors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();
        
            return new ErrorList(errors);
        }

        var employee = await _teamContract.CreateEmployeeAsync(cancellationToken);
        if (employee.IsFailure)
            return employee.Error;
        
        var employeeAccount = new EmployeeAccount(employeeUser, employee.Value);
        
        await _accountManager.CreateEmployeeAccountAsync(employeeAccount, cancellationToken);
    
        await _userManager.AddToRoleAsync(employeeUser, role!.Name!);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        transaction.Commit();
    
        return Result.Success<ErrorList>();
    }
}