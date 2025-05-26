using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

/// <summary>
/// Помогает определить, какую систему оценивания мы хотим:
/// 1. Численную
/// 2. Символьную
/// </summary>
public static class SkillGradeFactory
{
    public static Result<BaseSkillGrade, Error> CreateSkillGrade(
        SkillGradeId id,
        IEnumerable<object> grades,
        Name name,
        Description? description = null)
    {
        if (grades.Any() == false)
            return Errors.General.ValueIsRequired("grades cannot be empty");
        
        var sample = grades.First();

        switch (sample)
        {
            case int:
            {
                var intGrades = NumericSkillGrade.Create(id, grades, name, description);
                if (intGrades.IsFailure)
                    return intGrades.Error;
            
                return intGrades.Value;
            }
            case string:
            {
                var symbolGrades = SymbolsSkillGrade.Create(id, grades, name, description);
                if (symbolGrades.IsFailure)
                    return symbolGrades.Error;
            
                return symbolGrades.Value;
            }
            default:
                return Errors.General.ValueIsInvalid("Unknown skill grade type");
        }
    }
}