using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Performances.Infrastructure.DbContexts;
using TeamPulse.SharedKernel.Constants;

namespace TeamPulse.Performances.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPerformancesInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContexts(configuration);
        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(DatabaseConstant.DATABASE) 
                               ?? throw new ApplicationException("Cannot connect to the database.")));
        
        return services;
    }
}