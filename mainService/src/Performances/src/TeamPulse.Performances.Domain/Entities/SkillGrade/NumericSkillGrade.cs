using System.Text.Json;
using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public class NumericSkillGrade : BaseSkillGrade, IGrade<int>
{
    //ef core only
    private NumericSkillGrade()
    {
    }

    private NumericSkillGrade(
        SkillGradeId id,
        List<int> grades,
        Name name,
        Description description)
    {
        Id = id;
        _grades = grades;
        GradesAsString = JsonSerializer.Serialize(grades);
        Name = name;
        Description = description;
    }

    private List<int> _grades;

    public IReadOnlyList<int> Grades => _grades;

    public static Result<NumericSkillGrade, Error> Create(
        SkillGradeId id,
        IEnumerable<object> grades,
        Name name,
        Description description)
    {
        List<int> intGrades = [];

        foreach (var grade in grades)
        {
            var parseResult = int.TryParse(grade.ToString(), out var intGrade);
            
            if (parseResult == false)
                return Errors.General.ValueIsInvalid("Invalid Numeric Skill Grade.");

            intGrades.Add(intGrade);
        }

        return new NumericSkillGrade(id, intGrades, name, description);
    }


    public override IReadOnlyList<string> GetGradesAsString()
    {
        var grades = _grades.Select(g => g.ToString()).ToList();

        return grades;
    }
    
}