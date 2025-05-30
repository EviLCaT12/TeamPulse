using System.Text.Json;
using CSharpFunctionalExtensions;
using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Performances.Application.Factories;

/// <summary>
/// Помогает определить, какую систему оценивания мы хотим:
/// 1. Численную
/// 2. Символьную
/// </summary>
public static class SkillGradeFactory
{
    public static Result<BaseSkillGrade, Error> CreateSkillGrade(
        List<JsonElement> grades,
        Name name,
        Description? description = null)
    {
        if (grades.Count == 0)
            return Errors.General.ValueIsRequired("grades cannot be empty");
        
        var firstGrade = grades.First();

        var id = SkillGradeId.NewId();

        switch (firstGrade.ValueKind)
        {
            case JsonValueKind.Number:
            {
                var result = NumericSkillGrade.Create(
                    id,
                    grades.Select(g => (object)g),
                    name,
                    description);
            
                if (result.IsFailure)
                    return result.Error;

                return result.Value;
            }
            case JsonValueKind.String:
            {
                var result = SymbolsSkillGrade.Create(
                    id,
                    grades.Select(g => (object)g),
                    name,
                    description);
            
                if (result.IsFailure)
                    return result.Error;
            
                return result.Value;
            }
            
            default:
                return Errors.General.ValueIsInvalid("Unknown skill grade type");
        }
    }
}