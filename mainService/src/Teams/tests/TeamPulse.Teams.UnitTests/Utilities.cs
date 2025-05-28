using TeamPulse.SharedKernel.SharedVO;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.UnitTests;

public static class Utilities
{
    public static Department SeedDepartment()
    {
        var departmentId = DepartmentId.CreateNewId();
        var name = Name.Create(Guid.NewGuid().ToString()).Value;

        var department = new Department(departmentId, name, null, null);

        return department;
    }

    public static List<Team> SeedTeams(int count, Department department)
    {
        var teams = new List<Team>();
        for (var i = 0; i < count; i++)
        {
            var teamId = TeamId.CreateNewId();
            var name = Name.Create(Guid.NewGuid().ToString()).Value;
            var team = new Team(teamId, name, department, null, null);
            teams.Add(team);
        }

        return teams;
    }

    public static List<Employee> SeedEmployees(int count)
    {
        List<Employee> employees = [];
        for (int i = 0; i < count; i++)
        {
            var employeeId = EmployeeId.CreateNewId();
            var employee = new Employee(employeeId);
            employees.Add(employee);
        }
        
        return employees;
    }
}