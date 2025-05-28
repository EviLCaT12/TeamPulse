using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.ValueObjects.Ids;

namespace TeamPulse.Performances.Infrastructure.Configurations.Write;

public class GroupSkillConfiguration : IEntityTypeConfiguration<GroupSkill>
{
    public void Configure(EntityTypeBuilder<GroupSkill> builder)
    {
        builder.ToTable("group_skills");
        
        builder.HasKey(x => new { x.GroupId, x.SkillId });
        
        builder.Property(x => x.GroupId)
            .HasColumnName("group_id")
            .HasConversion(
                toDb => toDb.Value,
                fromDb => GroupOfSkillsId.Create(fromDb).Value);
        
        builder.Property(x => x.SkillId)
            .HasColumnName("skill_id")
            .HasConversion(
                toDb => toDb.Value,
                fromDb => SkillId.Create(fromDb).Value);

        builder
            .HasOne(x => x.GroupOfSkills)
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}