namespace TeamPulse.Teams.Contract.Requests.Department;

public record AddTeamsToDepartmentRequest(IEnumerable<Guid> TeamIds);