using TeamPulse.Accounts.Contracts;
using TeamPulse.Accounts.Infrastructure.Managers;

namespace TeamPulse.Accounts.Presentation;

public class AccountContract : IAccountContract
{
    private readonly PermissionManager _permissionManager;

    public AccountContract(PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }
    public async Task<HashSet<string>> GetUserPermissionCodes(Guid userId)
    {
        return await _permissionManager.GetUserPermissionCodes(userId);
    }

}