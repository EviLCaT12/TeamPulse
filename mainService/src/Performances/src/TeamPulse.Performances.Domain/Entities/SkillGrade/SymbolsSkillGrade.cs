using System.Text.Json;
using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

public class SymbolsSkillGrade : BaseSkillGrade, IGrade<string>
{
    //ef core only
    private SymbolsSkillGrade() { }
    
    private SymbolsSkillGrade(
        SkillGradeId id,
        IEnumerable<string> grades,
        Name name,
        Description? description = null)
    {
        Id = id;
        _grades = grades.ToList();
        GradesAsString = JsonSerializer.Serialize(grades);
        Name = name;
        Description = description;
    }
    
    private List<string> _grades;
    
    public IReadOnlyList<string> Grades => _grades;

    public static Result<SymbolsSkillGrade, Error> Create(
        SkillGradeId id,
        IEnumerable<object> grades,
        Name name,
        Description? description = null)
    {
        List<string> symbolsGrade = [];

        foreach (var grade in grades)
        {
            try
            {
                var parseResult = grade.ToString();
                symbolsGrade.Add(parseResult!);
            }
            catch 
            {
                return Errors.General.ValueIsInvalid($"Invalid string grade '{grade}'.");
            }
            
        }
        
        return new SymbolsSkillGrade(id, symbolsGrade, name, description);
    }

    
    public override IReadOnlyList<string> GetGradesAsString()
    {
        return _grades;
    }
}