using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Application.Commands.Department.Delete;
using TeamPulse.Teams.UnitTests;

namespace TeamPulse.Teams.IntegrationTests.Department;

public class Delete : BaseTest
{
    public Delete(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_Department_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        WriteDbContext.Departments.Add(department);

        var headOfTeam = Utilities.SeedEmployees(1).First();
        WriteDbContext.Employees.Add(headOfTeam);
        
        var team = Utilities.SeedTeams(1, department, headOfTeam);
        WriteDbContext.Teams.AddRange(team);
        
        WriteDbContext.SaveChanges();

        var command = new DeleteCommand(department.Id.Value);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<DeleteCommand>>();
        
        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();

        var isDepartmentDeleteFromDb = WriteDbContext.Departments.FirstOrDefault();
        isDepartmentDeleteFromDb.Should().BeNull();

    }
}