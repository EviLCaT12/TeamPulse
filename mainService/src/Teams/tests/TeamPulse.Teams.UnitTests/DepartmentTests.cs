using FluentAssertions;
using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.UnitTests;

public class DepartmentTests
{
    [Fact]
    public void Add_Teams_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var teams = Utilities.SeedTeams(5, department, headOfTeam);

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
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var team = Utilities.SeedTeams(1, department, headOfTeam).First();
        var employees = Utilities.SeedEmployees(5);
        
        department.AddTeams([team]);
        //Act
        var result = department.AddTeamEmployees(team.Id, employees);

        //Assert
        result.IsSuccess.Should().BeTrue();
        team.Employees.Should().BeEquivalentTo(employees);
    }

    [Fact]
    public void Add_Head_Of_Department_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        //Act;
        
        //Assert
        department.HeadOfDepartment.Should().NotBeNull();
        
        department.HeadOfDepartment.IsDepartmentManager.Should().BeTrue();
    }

    [Fact]
    public void Remove_Teams_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var teams = Utilities.SeedTeams(5, department, headOfTeam);
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
        var headOfTeams = Utilities.SeedEmployees(5);
        List<Team> oldTeams = [];
        List<Team> newTeams = [];
        foreach (var employee in headOfTeams)
        {
            var oldTeam = Utilities.SeedTeams(1, department, employee).First();
            oldTeams.Add(oldTeam);
            var newTeam = Utilities.SeedTeams(1, department, employee).First();
            newTeams.Add(newTeam);
        }
        
        department.AddTeams(oldTeams);
        
        //Act
        department.UpdateTeams(newTeams);
        
        //Assert
        department.Teams.Count.Should().Be(newTeams.Count);
        department.Teams.Should().BeEquivalentTo(newTeams);
    }

    [Fact]
    public void Update_Team_Name_Should_Be_Successful()
    {
        //Arrange
        var department = Utilities.SeedDepartment();
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var team = Utilities.SeedTeams(1, department,headOfTeam);
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
        var headOfTeam = Utilities.SeedEmployees(1).First();
        var team = Utilities.SeedTeams(1, department, headOfTeam).First();
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
        var oldHeadOfTeam = Utilities.SeedEmployees(1).First();
        var team = Utilities.SeedTeams(1, department, oldHeadOfTeam).First();
        var newHeadOfTeam = Utilities.SeedEmployees(1).First();
        
        department.AddTeams([team]);
        
        //Act
        department.UpdateHeadOfTeam(team, newHeadOfTeam);

        //Assert
        team.HeadOfTeam.Should().Be(newHeadOfTeam);
    }
}