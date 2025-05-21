using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public class NumericSkillGrade : IGrade
{
    private NumericSkillGrade(IEnumerable<int> grades)
    {
        _grades = grades.ToList();
    }
    
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
}