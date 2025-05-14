using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Application.Commands.Team.Create;
using TeamPulse.Teams.UnitTests;

namespace TeamPulse.Teams.IntegrationTests.Team;

public class Create : BaseTest
{
    public Create(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Team_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        WriteDbContext.Departments.Add(department);
        WriteDbContext.SaveChanges();

        var command = new CreateTeamCommand(
            Guid.NewGuid().ToString(),
            department.Id.Value,
            null,
            null);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateTeamCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var isTeamAddToDb = WriteDbContext.Teams.FirstOrDefault();
        isTeamAddToDb.Should().NotBeNull();
    }
}