using CSharpFunctionalExtensions;
using TeamPulse.Reports.Domain.Reports;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Reports.Domain;

/// <summary>
/// Семейство отчетов, которые высчитывают свои показатели на основе оценок менеджеров, поставленных их подчинённым
/// </summary>
/// <typeparam name="TGradeType">Тип оценки</typeparam> 
public interface IGradeReports<TGradeType>
{ 
    Result<BaseReport, Error> CreateReport(BaseReport report, Dictionary<Guid, TGradeType> gradeByEmployee);
}