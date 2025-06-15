using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TeamPulse.Accounts.Infrastructure.Seeding;

public class AccountsSeeder
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AccountsSeeder(IServiceScopeFactory serviceScopeFactory, ILogger<AccountsSeeder> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        
        using var scope = _serviceScopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();

        await service.SeedAsync(cancellationToken);
    }
    
}