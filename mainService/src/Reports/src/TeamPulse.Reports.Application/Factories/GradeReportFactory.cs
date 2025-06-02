using CSharpFunctionalExtensions;
using TeamPulse.Reports.Contract.Dtos;
using TeamPulse.Reports.Domain;
using TeamPulse.Reports.Domain.Reports;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Application.Factories;

public abstract class GradeReportFactory<TGradeType> : IReportFactory<Dictionary<Guid, TGradeType>>
{
    public Result<BaseReport, Error> GenerateReport(Dictionary<Guid, TGradeType> source, InfoToGenerateReport reportInfo)
    {
        switch (reportInfo.ReportName)
        {
            case "average_value":
                var report = new BaseReport(
                    ReportId.NewId(),
                    Name.Create(reportInfo.ReportName).Value,
                    Description.Create(reportInfo.Description).Value,
                    reportInfo.DepartmentId,
                    reportInfo.TeamId,
                    reportInfo.EmployeeId);
                
                    var result = MedianValueTeamReport<TGradeType>.Create(report, source);
                    
                    if (result.IsFailure)
                        return result.Error;
                
                    return result;
            
            default:
                return Errors.General.ValueIsInvalid("Unknown report name");
        }
    }
}