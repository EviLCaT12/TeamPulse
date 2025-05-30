namespace TeamPulse.Reports.Contract.Dtos;

public record InfoToGenerateReport(
    Guid DepartmentId,
    Guid TeamId,
    string ReportName,
    string Description);