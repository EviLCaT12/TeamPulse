using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Domain.Reports;

/// <summary>
/// Отчёт, дающий информацию о среднем показателе владения той или иной группой(команда/отдел)
/// какой-либо группой скиллов.
/// Если все три айди null - отчёт по сотруднику
/// EmployeeId is null - отчет по команде
/// TeamId is null - отчет по отделу 
/// </summary>
/// <typeparam name="TSourceType">Тип оценки</typeparam> 
public class MedianValueTeamReport<TSourceType> : BaseReport
{
    private MedianValueTeamReport(
        ReportId reportId,
        Name name,
        Description description,
        Guid departmentId,
        Guid? teamId,
        Guid? employeeId,
        Guid objectId,
        IEnumerable<TSourceType> sourceData,
        object medianValue) : base(reportId, name, description, departmentId, teamId, employeeId)
    {
        Object = objectId;
        _source = sourceData.ToList();
        MedianValue = medianValue;
    }
    
    
    public Guid Object { get; private set; }

    private List<TSourceType> _source = [];
    public IReadOnlyList<TSourceType> Source => _source;
    public object MedianValue { get; private set; }
    
    public static Result<BaseReport, Error> Create(
        BaseReport report,
        Guid objectId,
        IEnumerable<TSourceType> sourceData)
    {
        if (sourceData.Any() == false)
            return Errors.General.ValueIsRequired("Source data cannot be empty.");

        if (typeof(TSourceType) == typeof(int))
        {
            var intSource = sourceData.Cast<int>().ToList();

            decimal totalSum = intSource.Sum();
            var medianValue = totalSum / intSource.Count;

            return new MedianValueTeamReport<int>(
                report.Id,
                report.Name,
                report.Description,
                report.DepartmentId,
                report.TeamId,
                report.EmployeeId,
                objectId,
                intSource,
                medianValue);
        }

        if (typeof(TSourceType) == typeof(string))
        {
            var stringGrades = sourceData.Cast<string>().ToList();

            var mostFrequentString = stringGrades
                .GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
            
            // if (mostFrequentString.Count > 1)
            //     return Errors.General.ValueIsInvalid("More than mod frequent strings are not supported.");
            ;

            return new MedianValueTeamReport<string>(
                report.Id,
                report.Name,
                report.Description,
                report.DepartmentId,
                report.TeamId,
                report.EmployeeId,
                objectId,
                stringGrades,
                mostFrequentString!);
        }
          
        return Errors.General.ValueIsInvalid("Unknown grade type.");
    }
}