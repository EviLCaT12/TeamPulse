using FluentAssertions;

namespace TeamPulse.Teams.UnitTests;

public class DepartmentTests
{
    [Fact]
    public void Add_Teams_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var teams = Utilities.SeedTeams(5, department);

        //Act
        department.AddTeams(teams);

        //Assert
        department.Teams.Count.Should().Be(5);
    }
}