using CSharpFunctionalExtensions;
using TeamPulse.Reports.Contract.Dtos;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Domain.Reports;

public interface IReportFactory<in TSource>
{
    Result<BaseReport, Error> GenerateReport(TSource source, InfoToGenerateReport reportInfo);
}