namespace TeamPulse.Teams.Contract.Requests.Team;

public record UpdateTeamRequest(
    string? NewName,
    Guid? NewDepartmentId,
    IEnumerable<Guid>? NewEmployees,
    Guid? NewHeadOfTeam);