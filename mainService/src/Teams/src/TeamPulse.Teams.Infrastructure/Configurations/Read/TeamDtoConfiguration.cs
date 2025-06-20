using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Teams.Contract.Dtos;
using TeamPulse.Teams.Domain.Entities;

namespace TeamPulse.Teams.Infrastructure.Configurations.Read;

public class TeamDtoConfiguration : IEntityTypeConfiguration<TeamDto>
{
    public void Configure(EntityTypeBuilder<TeamDto> builder)
    {
        builder.ToTable("teams");
        
        builder.HasKey(t => t.Id);
        
        builder.HasMany(t => t.Employees)
            .WithOne()
            .HasForeignKey(e => e.Id)
            .IsRequired(false);
        
        
    }
}