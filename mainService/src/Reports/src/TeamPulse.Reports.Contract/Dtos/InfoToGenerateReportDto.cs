using TeamPulse.Reports.Domain.Enums;

namespace TeamPulse.Reports.Contract.Dtos;

public record InfoToGenerateReportDto(
    Guid DepartmentId,
    Guid? TeamId,
    Guid? EmployeeId,
    string Description,
    string Name,
    ReportType ReportType);