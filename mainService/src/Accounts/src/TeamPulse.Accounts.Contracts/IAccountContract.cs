namespace TeamPulse.Accounts.Contracts;

public interface IAccountContract
{
    Task<HashSet<string>> GetUserPermissionCodes(Guid userId);
}