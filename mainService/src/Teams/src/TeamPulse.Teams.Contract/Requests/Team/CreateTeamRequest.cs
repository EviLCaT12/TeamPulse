namespace TeamPulse.Teams.Contract.Requests.Team;

public record CreateTeamRequest(
    string Name,
    Guid DepartmentId,
    Guid HeadOfTeam);