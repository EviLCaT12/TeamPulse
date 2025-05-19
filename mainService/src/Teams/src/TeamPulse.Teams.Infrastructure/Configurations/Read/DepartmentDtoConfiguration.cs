using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Infrastructure.Configurations.Read;

public class DepartmentDtoConfiguration : IEntityTypeConfiguration<DepartmentDto>
{
    public void Configure(EntityTypeBuilder<DepartmentDto> builder)
    {
        builder.ToTable("departments");
        
        builder.HasKey(d => d.Id);

        builder.HasMany<TeamDto>(d => d.Teams)
            .WithOne()
            .HasForeignKey(t => t.DepartmentId)
            .IsRequired(false);
    }
}