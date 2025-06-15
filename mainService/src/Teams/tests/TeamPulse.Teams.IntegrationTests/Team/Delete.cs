using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Application.Commands.Team.Delete;
using TeamPulse.Teams.UnitTests;

namespace TeamPulse.Teams.IntegrationTests.Team;

public class Delete : BaseTest
{
    public Delete(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_Team_Should_Be_Successful()
    {
        //Arrange
        var employee = Utilities.SeedEmployees(1).First();
        WriteDbContext.Employees.Add(employee);
        WriteDbContext.SaveChanges();
        
        var department = Utilities.SeedDepartment(employee);
        
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var team = Utilities.SeedTeams(1, department, headOfTeam).First();
        department.AddTeams([team]);
        WriteDbContext.Departments.Add(department);
        WriteDbContext.Teams.Add(team);
        WriteDbContext.SaveChanges();
        
        var command = new DeleteCommand(team.Id.Value);
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<DeleteCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var deletedTeam = WriteDbContext.Teams.FirstOrDefault(t => t.Id == team.Id);
        deletedTeam.Should().BeNull();
    }
}