using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Accounts.Application;
using TeamPulse.Accounts.Application.AccountManagers;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Infrastructure.Contexts;
using TeamPulse.Accounts.Infrastructure.Jwt;
using TeamPulse.Accounts.Infrastructure.Managers;
using TeamPulse.Accounts.Infrastructure.Options;
using TeamPulse.Accounts.Infrastructure.Seeding;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Constants;


namespace TeamPulse.Accounts.Infrastructure;

public static  class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddUnitOfWork()
            .ConfigureCustomOptions(configuration)
            .AddIdentity()
            .AddSeeding()
            .AddTokenProviders();
        
        return services;
    }
    
    
    private static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        return services;
    }
    
    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(DatabaseConstant.DATABASE) 
                               ?? throw new ApplicationException("Cannot connect to the database.")));
        
        return services;
    }
    
    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.Account);
        return services;
    }
    
    private static IServiceCollection ConfigureCustomOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.SECTION_NAME));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SECTION_NAME));
        return services;
    }
    
    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => 
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<WriteDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<IAccountManager, AccountManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        return services;
    }

    private static IServiceCollection AddTokenProviders(this IServiceCollection services)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        return services;
    }
}