using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Application.Commands.Team.Update;
using TeamPulse.Teams.UnitTests;

namespace TeamPulse.Teams.IntegrationTests.Team;

public class Update : BaseTest
{
    public Update(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Full_Update_Team_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var oldHeadOfTeam = Utilities.SeedEmployees(1).First();
        var team = Utilities.SeedTeams(1, department, oldHeadOfTeam).First();
        var employees = Utilities.SeedEmployees(5);
        
        department.AddTeams([team]);
        department.AddTeamEmployees(team.Id, employees);
        WriteDbContext.Departments.Add(department);

        var newName = "new Name";
        var newDepartment = Utilities.SeedDepartment();
        var newEmployees = Utilities.SeedEmployees(7);
        var newHeadOfTeam = Utilities.SeedEmployees(1).First();

        WriteDbContext.Departments.Add(newDepartment);
        WriteDbContext.Employees.AddRange(newEmployees);
        WriteDbContext.Employees.Add(newHeadOfTeam);
        WriteDbContext.SaveChanges();
        
        var command = new UpdateCommand(
            team.Id.Value,
            newName,
            newDepartment.Id.Value,
            newEmployees.Select(e => e.Id.Value).ToList(),
            newHeadOfTeam.Id.Value);
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var updatedTeam = WriteDbContext.Teams.FirstOrDefault(t => t.Id == team.Id);
        updatedTeam.Should().NotBeNull();
        updatedTeam.Name.Value.Should().Be(newName);
        updatedTeam.DepartmentId.Should().Be(newDepartment.Id);
        updatedTeam.Employees.Should().BeEquivalentTo(newEmployees);
        updatedTeam.HeadOfTeam.Id.Should().Be(newHeadOfTeam.Id);
    }
}