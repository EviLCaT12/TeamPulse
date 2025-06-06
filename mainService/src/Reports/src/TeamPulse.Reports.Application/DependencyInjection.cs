using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;

namespace TeamPulse.Reports.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddReportsApplication(this IServiceCollection services)
    {
        services
            .AddCommands()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => 
                classes.AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}