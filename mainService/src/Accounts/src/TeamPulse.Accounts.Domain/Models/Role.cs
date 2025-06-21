using Microsoft.AspNetCore.Identity;

namespace TeamPulse.Accounts.Domain.Models;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; }
}