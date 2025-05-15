using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Teams.Contract;

namespace TeamPulse.Team.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddTeamPresentation(this IServiceCollection services)
    {
        services.AddScoped<ITeamContract, TeamContract>();
        return services;
    }
}