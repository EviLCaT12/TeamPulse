using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Infrastructure.Configurations.Write;

public class RecordSkillConfiguration : IEntityTypeConfiguration<RecordSkill>
{
    public void Configure(EntityTypeBuilder<RecordSkill> builder)
    {
        builder.ToTable("record_skills");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.RecordId)
            .IsRequired()
            .HasColumnName("record_id");
        
        builder
            .HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey("skill_id");
        
        builder.Property(x => x.SelfGrade)
            .IsRequired(false)
            .HasColumnName("self_grade");
        
        builder.Property(x => x.ManagerGrade)
            .IsRequired(false)
            .HasColumnName("manager_grade");
    }
}