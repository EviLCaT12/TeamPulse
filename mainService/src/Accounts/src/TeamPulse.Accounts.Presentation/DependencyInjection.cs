using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Accounts.Contracts;


namespace TeamPulse.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountContract, AccountContract>();

        return services;
    }
}