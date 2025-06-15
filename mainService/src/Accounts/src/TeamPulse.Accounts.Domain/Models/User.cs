using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using TeamPulse.Accounts.Domain.Models.AccountModels;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Accounts.Domain.Models;

public class User : IdentityUser<Guid>
{
    private User()
    {
    }
    
    private List<Role> _roles = [];
    
    public IReadOnlyList<Role> Roles => _roles;
    
    public EmployeeAccount? EmployeeAccount { get; set; }
    
    public AdminAccount? AdminAccount { get; set; }
    
    
    public static Result<User, ErrorList> CreateEmployee(
        string userName,
        string email,
        Role role)
    {
        if (role.Name != EmployeeAccount.Employee)
            return Errors.General.ValueIsInvalid(nameof(role)).ToErrorList();
        
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }
    
    public static Result<User, ErrorList> CreateAdmin(
        string userName,
        string email,
        Role role)
    {
        if (role.Name != AdminAccount.Admin)
            return Errors.General.ValueIsInvalid(nameof(role)).ToErrorList();
        
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }
}