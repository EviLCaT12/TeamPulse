using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public class SymbolsSkillGrade : IGrade<string>
{
    private SymbolsSkillGrade(IEnumerable<string> grades)
    {
        _grades = grades.ToList();
    }
    
    public SkillGradeId Id { get; private set; }
    
    private List<string> _grades;
    
    public IReadOnlyList<string> Grades => _grades;

    public static Result<SymbolsSkillGrade, Error> Create(IEnumerable<object> grades)
    {
        List<string> symbolsGrade = [];

        foreach (var grade in grades)
        {
            if (grade is not string stringGrade)
                return Errors.General.ValueIsInvalid($"Invalid string grade '{grade}'.");
            
            symbolsGrade.Add(stringGrade);
        }
        
        return new SymbolsSkillGrade(symbolsGrade);
    }

    
    public IReadOnlyList<string> GetGradesAsString()
    {
        return _grades;
    }
}