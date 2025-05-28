using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Infrastructure.Configurations.Write;

public class RecordSkillConfiguration : IEntityTypeConfiguration<RecordSkill>
{
    public void Configure(EntityTypeBuilder<RecordSkill> builder)
    {
        builder.ToTable("record_skills");
        
        builder.HasKey(x => new {x.WhoId, x.WhatId});

        builder.Property(x => x.WhoId)
            .HasColumnName("who_id")
            .IsRequired();
        
        builder.Property(x => x.WhatId)
            .HasColumnName("what_id")
            .IsRequired();
        
        builder.Property(x => x.SelfGrade)
            .IsRequired(false)
            .HasColumnName("self_grade");
        
        builder.Property(x => x.ManagerGrade)
            .IsRequired(false)
            .HasColumnName("manager_grade");
    }
}