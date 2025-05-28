using FluentAssertions;
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
        var department = Utilities.SeedDepartment();
        var teams = Utilities.SeedTeams(3, department);
        var headOfDepartment = Utilities.SeedEmployees(1).First();
        
        department.AddTeams(teams);
        department.AddHeadOfDepartment(headOfDepartment);
        WriteDbContext.Departments.Add(department);
        
        
        var newName = "new name";
        var newTeams = Utilities.SeedTeams(3, department);
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
        
        var newDepartment = WriteDbContext.Departments.SingleOrDefault(d => d.Id == department.Id);
        newDepartment.Should().NotBeNull();
        newDepartment.Name.Value.Should().Be(newName);
        newDepartment.Teams.Should().BeEquivalentTo(newTeams);
        newDepartment.HeadOfDepartment!.Id.Should().Be(newHeadOfDepartment.Id);
    }
}