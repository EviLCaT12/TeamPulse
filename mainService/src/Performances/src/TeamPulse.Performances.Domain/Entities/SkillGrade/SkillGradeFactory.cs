using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Performances.Domain.Entities.SkillGrade;

/// <summary>
/// Помогает определить, какую систему оценивания мы хотим:
/// 1. Численную
/// 2. Символьную
/// </summary>
public static class SkillGradeFactory
{
    public static Result<IGrade, Error> CreateSkillGrade(IEnumerable<object> grades)
    {
        if (grades.Any() == false)
            return Errors.General.ValueIsRequired("grades cannot be empty");
        
        var sample = grades.First();

        if (sample is int)
        {
            var intGrades = NumericSkillGrade.Create(grades);
            if (intGrades.IsFailure)
                return intGrades.Error;
            
            return intGrades.Value;
        }

        if (sample is string)
        {
            var symbolGrades = SymbolsSkillGrade.Create(grades);
            if (symbolGrades.IsFailure)
                return symbolGrades.Error;
            
            return symbolGrades.Value;
        }
        
        return Errors.General.ValueIsInvalid("Unknown skill grade type");
    }
}