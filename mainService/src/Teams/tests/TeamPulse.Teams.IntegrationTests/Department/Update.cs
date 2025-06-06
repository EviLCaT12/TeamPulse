using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Application.Commands.Department.Update;
using TeamPulse.Teams.UnitTests;

namespace TeamPulse.Teams.IntegrationTests.Department;

public class Update : BaseTest
{
    public Update(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Full_Update_Department_Should_Be_Successful()
    {
        //Arrange
        var headOfDepartment = Utilities.SeedEmployees(1).First();
        WriteDbContext.Employees.Add(headOfDepartment);
        WriteDbContext.SaveChanges();
        
        var department = Utilities.SeedDepartment(headOfDepartment);
        var headOfTeams = Utilities.SeedEmployees(3);
        List<Domain.Entities.Team> oldTeams = [];
        foreach (var employee in headOfTeams)
        {
            var team = Utilities.SeedTeams(1, department, employee).First();
            oldTeams.Add(team);
        }
        
        department.AddTeams(oldTeams);
        WriteDbContext.Departments.Add(department);
        
        
        var newName = "new name";
        var newHeadOfTeams = Utilities.SeedEmployees(3);
        List<Domain.Entities.Team> newTeams = [];
        foreach (var employee in newHeadOfTeams)
        {
            var team = Utilities.SeedTeams(1, department, employee).First();
            newTeams.Add(team);
        }
        WriteDbContext.Teams.AddRange(newTeams);
        var newHeadOfDepartment = Utilities.SeedEmployees(1).First();
        WriteDbContext.Employees.AddRange(newHeadOfDepartment);
        
        WriteDbContext.SaveChanges();

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateCommand>>();
        var command = new UpdateCommand(
            department.Id.Value,
            newName,
            newTeams.Select(t => t.Id.Value).AsEnumerable(),
            newHeadOfDepartment.Id.Value);
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        
        var newDepartment = WriteDbContext.Departments.FirstOrDefault(dep => dep.Id == department.Id);
        newDepartment.Should().NotBeNull();
        newDepartment.Name.Value.Should().Be(newName);
        newDepartment.Teams.Should().BeEquivalentTo(newTeams);
        newDepartment.HeadOfDepartment!.Id.Should().Be(newHeadOfDepartment.Id);
    }
}