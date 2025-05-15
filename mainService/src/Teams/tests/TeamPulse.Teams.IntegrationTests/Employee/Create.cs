using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TeamPulse.Core.Abstractions;
using TeamPulse.Teams.Application.Commands.Employee.Create;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.IntegrationTests.Employee;

public class Create : BaseTest
{
    public Create(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_Employee_Should_Be_Successful()
    {
        //Arrange
        var command = new CreateCommand();
        var sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateCommand>>();

        //Act
        var result = await sut.HandleAsync(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var isEmployeeAddToDb = WriteDbContext.Employees.FirstOrDefault();
        isEmployeeAddToDb.Should().NotBeNull();
    }
}