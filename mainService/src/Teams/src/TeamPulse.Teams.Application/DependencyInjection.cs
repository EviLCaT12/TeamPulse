using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using FluentValidation;

namespace TeamPulse.Teams.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTeamApplication(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly)
            .AddCommands();
        
        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}