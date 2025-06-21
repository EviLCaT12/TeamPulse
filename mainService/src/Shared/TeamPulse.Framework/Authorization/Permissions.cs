namespace TeamPulse.Framework.Authorization;

public static class Permissions
{
    public static class Team
    {
        public const string CreateDepartment = "department.create";
        public const string UpdateDepartment = "department.update";
        public const string DeleteDepartment = "department.delete";
        public const string GetDepartment = "department.get";
        public const string AddTeamsToDepartment = "department.add.teams";
        
        
        public const string CreateEmployee = "employee.create";
        public const string GetEmployee = "employee.get";
        
        
        public const string CreateTeam = "team.create";
        public const string UpdateTeam = "team.update";
        public const string DeleteTeam = "team.delete";
        public const string GetTeams = "team.get";
        public const string AddEmployeesToTeam = "team.add.employees";
    }

    public static class Performances
    {
        public const string GetGroupOfSkills = "group.get";
        public const string CreateGroupOfSkills = "group.create";
        public const string AddSkillToGroup = "group.add.skills";
        
        
        public const string CreateSkill = "skill.create";
        
        
        public const string CreateSkillGrade = "grade.create";
        public const string EmployeeSelfReview = "employee.self.review";
    }


    public static class Reports
    {
        public const string GetMedianValueReports = "median.value.get";
    }
}