using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Infrastructure.Configurations.Read;

public class SkillGradeDtoConfiguration : IEntityTypeConfiguration<SkillGradeDto>
{
    public void Configure(EntityTypeBuilder<SkillGradeDto> builder)
    {
        builder.ToTable("skill_grade");

        builder.HasKey(x => x.Id);
        
        
    }
}