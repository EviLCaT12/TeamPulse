using CSharpFunctionalExtensions;
using TeamPulse.Reports.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Domain.Reports;

public abstract class GradeReportFactory<TGradeType> : IReportFactory<Dictionary<Guid, TGradeType>>
{
    public Result<BaseReport, Error> GenerateReport(Dictionary<Guid, TGradeType> source, InfoToGenerateReport reportInfo)
    {
        switch (reportInfo.ReportName)
        {
            case "average_value":
                var report = new BaseReport(
                    ReportId.NewId(),
                    reportInfo.DepartmentId,
                    reportInfo.TeamId,
                    Name.Create(reportInfo.ReportName).Value,
                    Description.Create(reportInfo.Description).Value);
                
                    var result = MedianValueReport<TGradeType>.Create(report, source);
                    
                    if (result.IsFailure)
                        return result.Error;
                
                    return result;
            
            default:
                return Errors.General.ValueIsInvalid("Unknown report name");
        }
    }
}