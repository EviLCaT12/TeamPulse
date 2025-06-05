using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Contract.Dtos;
using TeamPulse.Performances.Domain.Entities;

namespace TeamPulse.Performances.Infrastructure.Configurations.Read;

public class GroupSkillDtoConfiguration : IEntityTypeConfiguration<GroupSkillDto>
{
    public void Configure(EntityTypeBuilder<GroupSkillDto> builder)
    {
        builder.ToTable("group_skills");
        
        builder.HasKey(x => new { x.GroupId, x.SkillId });

        builder.HasOne(x => x.Group)
            .WithMany()
            .HasForeignKey(x => x.GroupId);
        
        builder.HasOne(x => x.Skill)
            .WithMany()
            .HasForeignKey(x => x.SkillId);
    }
}