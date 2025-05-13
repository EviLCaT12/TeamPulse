using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Teams.Infrastructure.DbContexts;

namespace TeamPulse.Teams.IntegrationTests.Department;

public class DepartmentBaseTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IServiceScope Scope;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly Fixture Fixture;

    public DepartmentBaseTest(IntegrationTestsWebFactory factory)
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