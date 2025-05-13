using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Application.Commands.Department.Create;

namespace TeamPulse.Teams.IntegrationTests.Department;

public class Create : DepartmentBaseTest
{
    public Create(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Department_Should_Be_Successful()
    {
        //Arrange
        var command = new CreateDepartmentCommand(Guid.NewGuid().ToString(), null, null);

        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateDepartmentCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var isValueAddToDatabase = WriteDbContext.Departments.First();
        isValueAddToDatabase.Should().NotBeNull();
    }
}