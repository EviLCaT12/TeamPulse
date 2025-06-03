using TeamPulse.Reports.Domain.Enums;

namespace TeamPulse.Reports.Contract.Requests;

public record GetMedianValueReportsRequest(
    string Name,
    string Description,
    int ObjectType);