namespace TeamPulse.Teams.Contract.Requests.Department;

public record CreateDepartmentRequest(
    string Name,
    IEnumerable<Guid>? Teams,
    Guid HeadOfDepartment);