using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Domain.Entities.SkillGrade;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Infrastructure.Configurations.Write;

public class SkillGradeConfiguration : IEntityTypeConfiguration<BaseSkillGrade>
{
    public void Configure(EntityTypeBuilder<BaseSkillGrade> builder)
    {
        builder.ToTable("skill_grade");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasConversion(
                toDb => toDb.Value,
                fromDb => SkillGradeId.Create(fromDb).Value);

        builder.HasDiscriminator<string>("grade_type")
            .HasValue<NumericSkillGrade>("numeric_grade")
            .HasValue<SymbolsSkillGrade>("symbol_grade");

        builder.Property(x => x.GradesAsString)
            .HasColumnName("grades")
            .HasColumnType("jsonb");

        builder.ComplexProperty(x => x.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("grade_name")
                .IsRequired();
        });

        builder.ComplexProperty(x => x.Description, db =>
        {
            db.Property(d => d.Value)
                .HasColumnName("grade_description")
                .IsRequired();
        });
    }
}