using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Domain.Reports;

/// <summary>
/// Отчёт, дающий информацию о среднем показателе владения той или иной группой(команда/отдел)
/// какой-либо группой скиллов
/// </summary>
/// <typeparam name="TGradeType">Тип оценки</typeparam> 
public class MedianValueReport<TGradeType> : BaseReport
{
    private MedianValueReport(
        ReportId reportId,
        Guid departmentId,
        Guid teamId,
        Name name,
        Description description,
        Dictionary<Guid, TGradeType> gradesByEmployees,
        object medianValue) : base(reportId, departmentId, teamId, name, description)
    {
        GradeByEmployee = gradesByEmployees;
        MedianValue = medianValue;
    }

    public Dictionary<Guid, TGradeType> GradeByEmployee { get; private set; }
    public object MedianValue { get; private set; }


    public static Result<BaseReport, Error> Create(BaseReport report, Dictionary<Guid, TGradeType> gradeByEmployee)
    {
        if (gradeByEmployee.Count == 0)
            return Errors.General.ValueIsRequired("Employees and grades cannot be empty.");

        if (typeof(TGradeType) == typeof(int))
        {
            decimal totalSum = gradeByEmployee.Values
                .Aggregate(0, (current, grade) => current + (int)(object)grade!);
            
            var medianValue = totalSum / gradeByEmployee.Count;

            var intGrades = gradeByEmployee.ToDictionary(
                kvp => kvp.Key,
                kvp => (int)(object)kvp.Value!);
            
            return new MedianValueReport<int>(
                report.Id,
                report.DepartmentId,
                report.TeamId,
                report.Name,
                report.Description,
                intGrades,
                medianValue);
        }

        if (typeof(TGradeType) == typeof(string))
        {
            var stringGrades = gradeByEmployee.Values.Cast<string>().ToList();

            var mostFrequentString = stringGrades
                .GroupBy(s => s)
                .OrderByDescending(s => s.Count())
                .Select(g => g.Key)
                .ToList();
            
            if (mostFrequentString.Count > 1)
                return Errors.General.ValueIsInvalid("More than mod frequent strings are not supported.");

            var medianValue = mostFrequentString.First();
            
            var stringGradesDict = gradeByEmployee.ToDictionary(
                kvp => kvp.Key,
                kvp => (string)(object)kvp.Value!);
            
            return new MedianValueReport<string>(
                report.Id,
                report.DepartmentId,
                report.TeamId,
                report.Name,
                report.Description,
                stringGradesDict,
                medianValue);
        }
        
        return Errors.General.ValueIsInvalid("Unknown grade type.");
    }
}