using CSharpFunctionalExtensions;
using TeamPulse.Reports.Contract.Dtos;
using TeamPulse.Reports.Domain.Reports;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Reports.Application.Factories;

public interface IReportFactory<in TSource>
{
    Result<BaseReport, Error> GenerateReport(TSource source, InfoToGenerateReport reportInfo);
}