namespace TeamPulse.Reports.Contract.Dtos;

public record InfoToGenerateReport(
    Guid DepartmentId,
    Guid? TeamId,
    Guid? EmployeeId,
    string ReportName,
    string Description);