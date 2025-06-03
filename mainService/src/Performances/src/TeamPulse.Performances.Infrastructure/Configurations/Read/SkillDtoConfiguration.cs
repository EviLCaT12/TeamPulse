using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Infrastructure.Configurations.Read;

public class SkillDtoConfiguration : IEntityTypeConfiguration<SkillDto>
{
    public void Configure(EntityTypeBuilder<SkillDto> builder)
    {
        builder.ToTable("skills");
        
        builder.HasKey(s => s.Id);
        
        builder.HasOne(s => s.Grade)
            .WithMany()
            .HasForeignKey("grade_id");
    }
}