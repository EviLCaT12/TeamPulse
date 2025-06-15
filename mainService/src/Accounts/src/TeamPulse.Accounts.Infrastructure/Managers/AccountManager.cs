using Microsoft.EntityFrameworkCore;
using TeamPulse.Accounts.Application.AccountManagers;
using TeamPulse.Accounts.Domain.Models.AccountModels;
using TeamPulse.Accounts.Infrastructure.Contexts;

namespace TeamPulse.Accounts.Infrastructure.Managers;

public class AccountManager(WriteDbContext context) : IAccountManager
{
    public async Task CreateEmployeeAccountAsync(EmployeeAccount employeeAccount, CancellationToken cancellationToken)
    {
        await context.EmployeeAccounts.AddAsync(employeeAccount, cancellationToken);
    }

    public async Task CreateAdminAccountAsync(AdminAccount adminAccount)
    {
        await context.AdminAccounts.AddAsync(adminAccount);
    }
}