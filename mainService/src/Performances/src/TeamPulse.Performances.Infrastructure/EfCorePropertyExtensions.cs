using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamPulse.Performances.Infrastructure;
/// <summary>
/// Заготовка на прекрасное будущее
/// </summary>
public static class EfCorePropertyExtensions
{
    public static PropertyBuilder<IReadOnlyList<TGrades>> JsonGradeCollectionConverter<TGrades>(
        this PropertyBuilder<IReadOnlyList<TGrades>> builder)
    {
        return builder.HasConversion(
            gradesToDb => SerializeGradeCollection(gradesToDb),
            gradesFromDb => DeserializeGradeCollection<TGrades>(gradesFromDb),
            new ValueComparer<IReadOnlyList<TGrades>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()));
    }

    private static string SerializeGradeCollection<TGrades>(IReadOnlyList<TGrades> grades)
    {
        return JsonSerializer.Serialize(grades, JsonSerializerOptions.Default);
    }

    private static List<TGrades> DeserializeGradeCollection<TGrades>(string json)
    {
        var grades = JsonSerializer.Deserialize<List<TGrades>>(json, JsonSerializerOptions.Default) ?? [];
        
        return grades;
    }
}