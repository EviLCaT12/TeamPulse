using CSharpFunctionalExtensions;
using TeamPulse.Reports.Contract.Dtos;
using TeamPulse.Reports.Domain;
using TeamPulse.Reports.Domain.Enums;
using TeamPulse.Reports.Domain.Reports;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Application.Factories;

public abstract class GradeReportFactory<TSourceType>
{
    public static Result<BaseReport, Error> GenerateReport(Guid objectId, IEnumerable<TSourceType> source, InfoToGenerateReportDto reportInfo)
    {
        switch (reportInfo.ReportType)
        {
            case ReportType.MedianValue:
                var report = new BaseReport(
                    ReportId.NewId(),
                    Name.Create(reportInfo.Name).Value,
                    Description.Create(reportInfo.Description).Value,
                    reportInfo.DepartmentId,
                    reportInfo.TeamId,
                    reportInfo.EmployeeId);
                
                    var result = MedianValueTeamReport<TSourceType>.Create(report, objectId, source);
                    
                    if (result.IsFailure)
                        return result.Error;
                
                    return result;
            
            default:
                return Errors.General.ValueIsInvalid("Unknown report name");
        }
    }
}