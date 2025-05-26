using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Infrastructure.Configurations.Write;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("skills");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasConversion(
                toDb => toDb.Value,
                fromDb => SkillId.Create(fromDb).Value);

        builder.ComplexProperty(s => s.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name");
        });
        
        builder.ComplexProperty(s => s.Description, descBuilder =>
        {
            descBuilder.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("description");
        });
        
        builder.HasOne(s => s.SkillGrade)
            .WithMany()
            .HasForeignKey("grade_id");
    }
}