namespace TeamPulse.Teams.Contract.Requests.Team;

public record AddEmployeeRequest(IEnumerable<Guid> EmployeeIds);