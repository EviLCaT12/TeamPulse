using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories;
using TeamPulse.Performances.Infrastructure.DbContexts;
using TeamPulse.Performances.Infrastructure.Repositories;
using TeamPulse.SharedKernel.Constants;

namespace TeamPulse.Performances.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPerformancesInfrastructure(this IServiceCollection services,
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

        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(DatabaseConstant.DATABASE)
                              ?? throw new ApplicationException("Cannot connect to the database.")));
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.Performance);
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<ISkillGradeRepository, SkillGradeRepository>()
            .AddScoped<ISkillRepository, SkillRepository>()
            .AddScoped<IGroupOfSkillRepository, GroupOfSkillRepository>()
            .AddScoped<IRecordSkillRepository, RecordSkillRepository>()
            .AddScoped<IGroupSkillRepository, GroupSkillRepository>();
        
        return services;
    }
}