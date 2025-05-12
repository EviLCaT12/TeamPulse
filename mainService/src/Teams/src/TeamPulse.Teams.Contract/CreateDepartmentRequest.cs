namespace TeamPulse.Teams.Contract;

public record CreateDepartmentRequest(
    string Name,
    IEnumerable<Guid>? Teams,
    Guid? HeadOfDepartment);