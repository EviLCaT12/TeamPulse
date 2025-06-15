using TeamPulse.Accounts.Domain.Models.AccountModels;

namespace TeamPulse.Accounts.Application.AccountManagers;

public interface IAccountManager
{ 
    Task CreateEmployeeAccountAsync(EmployeeAccount employeeAccount, CancellationToken cancellationToken);
    
    Task CreateAdminAccountAsync(AdminAccount adminAccount);
}