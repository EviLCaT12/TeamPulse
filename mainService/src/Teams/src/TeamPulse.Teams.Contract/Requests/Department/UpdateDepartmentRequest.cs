namespace TeamPulse.Teams.Contract.Requests.Department;

public record UpdateDepartmentRequest(    
    string? NewName,
    IEnumerable<Guid>? NewTeams,
    Guid? NewHeadOfDepartment);