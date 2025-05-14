using FluentAssertions;
using TeamPulse.SharedKernel.SharedVO;

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

    [Fact]
    public void Update_Name_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var newName = Name.Create("New Name").Value;
        
        //Act
        department.UpdateName(newName);
        
        //Assert
        department.Name.Should().Be(newName);
    }
    
    [Fact]
    public void Update_Teams__Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var oldTeams = Utilities.SeedTeams(5, department); 
        var newTeams = Utilities.SeedTeams(10, department);
        
        department.AddTeams(oldTeams);
        
        //Act
        department.UpdateTeams(newTeams);
        
        //Assert
        department.Teams.Count.Should().Be(10);
        department.Teams.Should().BeEquivalentTo(newTeams);
    }
}