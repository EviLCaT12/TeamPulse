using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.Constants;
using TeamPulse.Teams.Application;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Teams.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Teams.Infrastructure.DbContexts;
using TeamPulse.Teams.Infrastructure.Repositories;
using TeamPulse.Teams.Infrastructure.Repositories.Read;
using TeamPulse.Teams.Infrastructure.Repositories.Write;

namespace TeamPulse.Teams.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTeamInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddUnitOfWork()
            .AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(DatabaseConstant.DATABASE) 
                               ?? throw new ApplicationException("Cannot connect to the database.")));

        services.AddScoped<ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(DatabaseConstant.DATABASE)
                              ?? throw new ApplicationException("Cannot connect to the database.")));
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.Team);
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        //Write
        services
            .AddScoped<IEmployeeWriteRepository, EmployeeWriteRepository>()
            .AddScoped<ITeamWriteRepository, TeamWriteRepository>()
            .AddScoped<IDepartmentWriteRepository, DepartmentWriteRepository>();
        
        //Read
        services
            .AddScoped<IDepartmentReadRepository, DepartmentReadRepository>()
            .AddScoped<ITeamReadRepository, TeamReadRepository>()
            .AddScoped<IEmployeeReadRepository, EmployeeReadRepository>();
        
        return services;
    }
}