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
    public void Add_Team_Employee_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var team = Utilities.SeedTeams(1, department).First();
        var employees = Utilities.SeedEmployees(5);
        
        department.AddTeams([team]);
        //Act
        var result = department.AddTeamEmployees(team.Id, employees);

        //Assert
        result.IsSuccess.Should().BeTrue();
        team.Employees.Should().BeEquivalentTo(employees);
    }

    [Fact]
    public void Add_Head_Of_Team_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var team = Utilities.SeedTeams(1, department).First();
        var headOfTeam = Utilities.SeedEmployees(1).First();

        department.AddTeams([team]);
        
        //Act
        var result = department.AddHeadOfTeam(team.Id, headOfTeam);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        team.HeadOfTeam.Should().Be(headOfTeam);
    }

    [Fact]
    public void Add_Head_Of_Department_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var headOfDepartment = Utilities.SeedEmployees(1).First();
        
        //Act
        department.AddHeadOfDepartment(headOfDepartment);
        
        //Assert
        department.HeadOfDepartment.Should().Be(headOfDepartment);
    }

    [Fact]
    public void Remove_Teams_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var teams = Utilities.SeedTeams(5, department);
        department.AddTeams(teams);
        
        //Act
        department.RemoveTeam(teams[0]);
        
        //Assert
        department.Teams.Count.Should().Be(4);
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
    public void Update_Teams_Should_Be_Successful()
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

    [Fact]
    public void Update_Team_Name_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var team = Utilities.SeedTeams(1, department);
        var newName = Name.Create("New Name").Value;
        department.AddTeams(team);
        
        //Act
        var result = department.UpdateTeamName(team.First().Id, newName);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        team.First().Name.Should().Be(newName);
    }

    [Fact]
    public void Update_Team_Employees_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var team = Utilities.SeedTeams(1, department).First();
        var employees = Utilities.SeedEmployees(5);
        var newEmployees = Utilities.SeedEmployees(7);
        
        department.AddTeams([team]);
        department.AddTeamEmployees(team.Id, employees);
        //Act
        var result = department.UpdateTeamEmployees(team.Id, newEmployees);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        team.Employees.Should().BeEquivalentTo(newEmployees);
    }

    [Fact]
    public void Update_Team_Head_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var team = Utilities.SeedTeams(1, department).First();
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var newHeadOfTeam = Utilities.SeedEmployees(1).First();
        
        department.AddTeams([team]);
        department.AddHeadOfTeam(team.Id, headOfTeam);
        
        //Act
        var result = department.UpdateHeadOfTeam(team.Id, newHeadOfTeam);

        //Assert
        result.IsSuccess.Should().BeTrue();
        team.HeadOfTeam.Should().Be(newHeadOfTeam);
    }
}