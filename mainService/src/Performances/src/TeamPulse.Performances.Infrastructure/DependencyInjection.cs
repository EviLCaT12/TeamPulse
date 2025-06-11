using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Read;
using TeamPulse.Performances.Application.DatabaseAbstraction.Repositories.Write;
using TeamPulse.Performances.Infrastructure.DbContexts;
using TeamPulse.Performances.Infrastructure.Repositories;
using TeamPulse.Performances.Infrastructure.Repositories.Read;
using TeamPulse.Performances.Infrastructure.Repositories.Write;
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

        services.AddScoped<ReadDbContext>(_ =>
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
        //write
        services
            .AddScoped<ISkillGradeWriteRepository, SkillGradeWriteRepository>()
            .AddScoped<ISkillWriteRepository, SkillWriteRepository>()
            .AddScoped<IGroupOfSkillWriteRepository, GroupOfSkillWriteRepository>()
            .AddScoped<IRecordSkillWriteRepository, RecordSkillWriteRepository>()
            .AddScoped<IGroupSkillWriteRepository, GroupSkillWriteRepository>();
        
        //Read
        services
            .AddScoped<IGroupOfSkillReadRepository, GroupOfSkillReadRepository>()
            .AddScoped<IGroupSkillReadRepository, GroupSkillReadRepository>()
            .AddScoped<IRecordSkillReadRepository, RecordSkillReadRepository>();
        return services;
    }
}