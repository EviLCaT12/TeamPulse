namespace TeamPulse.Teams.Contract;

public record UpdateDepartmentRequest(    
    string NewName,
    IEnumerable<Guid>? NewTeams,
    Guid? NewHeadOfDepartment);