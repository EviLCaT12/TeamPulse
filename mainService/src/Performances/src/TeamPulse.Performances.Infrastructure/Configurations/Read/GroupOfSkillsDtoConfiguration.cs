using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Infrastructure.Configurations.Read;

public class GroupOfSkillsDtoConfiguration : IEntityTypeConfiguration<GroupOfSkillsDto>
{
    public void Configure(EntityTypeBuilder<GroupOfSkillsDto> builder)
    {
        builder.ToTable("group_of_skills");

        builder.HasKey(g => g.Id);

        builder.HasOne(g => g.SkillGrade)
            .WithMany()
            .HasForeignKey("grade_id");
    }
}