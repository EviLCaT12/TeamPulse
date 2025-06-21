using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Domain.Models.AccountModels;

namespace TeamPulse.Accounts.Infrastructure.Contexts;

public class WriteDbContext(string connectionString) 
    : IdentityDbContext<User, Role, Guid>
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    
    public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
    
    public DbSet<EmployeeAccount> EmployeeAccounts => Set<EmployeeAccount>();
    
    public DbSet<AdminAccount> AdminAccounts => Set<AdminAccount>();
    

    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Role>()
            .ToTable("roles");
         
        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");
        
        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");
        
        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");
        
        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");
        
        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
        
        
        builder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        builder.HasDefaultSchema("accounts");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create((builder) => {builder.AddConsole();});
}