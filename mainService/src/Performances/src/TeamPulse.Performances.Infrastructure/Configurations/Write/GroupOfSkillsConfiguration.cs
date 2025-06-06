using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Infrastructure.Configurations.Write;

public class GroupOfSkillsConfiguration : IEntityTypeConfiguration<GroupOfSkills>
{
    public void Configure(EntityTypeBuilder<GroupOfSkills> builder)
    {
        builder.ToTable("group_of_skills");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                toDb => toDb.Value,
                fromDb => GroupOfSkillsId.Create(fromDb).Value);

        builder.ComplexProperty(x => x.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("group_name")
                .IsRequired();
        });

        builder.ComplexProperty(x => x.Description, db =>
        {
            db.Property(n => n.Value)
                .HasColumnName("group_description")
                .IsRequired();
        });
        
        builder.HasOne(x => x.SkillGrade)
            .WithMany()
            .HasForeignKey("grade_id");
        
    }
}