using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Performances.Contract;

namespace TeamPulse.Performances.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPerformancesPresentation(this IServiceCollection services)
    {
        services.AddScoped<IPerformanceContract, PerformanceContract>();
        
        return services;
    }
}