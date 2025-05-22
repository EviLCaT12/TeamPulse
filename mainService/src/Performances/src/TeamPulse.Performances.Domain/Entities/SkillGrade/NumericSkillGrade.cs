using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public class NumericSkillGrade : IGrade<int>
{
    private NumericSkillGrade(IEnumerable<int> grades)
    {
        _grades = grades.ToList();
    }
    
    public SkillGradeId Id { get; private set; }
    
    private List<int> _grades;
    
    public IReadOnlyList<int> Grades => _grades;

    public static Result<NumericSkillGrade, Error> Create(IEnumerable<object> grades)
    {
        List<int> intGrades = [];

        foreach (var grade in grades)
        {
            if (grade is not int intGrade)
                return Errors.General.ValueIsInvalid("Invalid Numeric Skill Grade.");
            
            intGrades.Add(intGrade);
        }

        return new NumericSkillGrade(intGrades);
    }

    
    public IReadOnlyList<string> GetGradesAsString()
    {
        var grades = _grades.Select(g => g.ToString()).ToList();
        
        return grades;
    }
}