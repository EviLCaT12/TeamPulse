using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.IntegrationTests;

public class BaseTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IServiceScope Scope;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly Fixture Fixture;

    public BaseTest(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = Factory.Services.CreateScope();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Fixture = new Fixture();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    async Task IAsyncLifetime.DisposeAsync()
    {
        await Factory.ResetDatabaseAsync();
        Scope.Dispose();
    }
}