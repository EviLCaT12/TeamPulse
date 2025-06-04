using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.SharedKernel.Constants;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Infrastructure.Configurations.Write;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasConversion(
                idToDb => idToDb.Value,
                idFromDb => TeamId.Create(idFromDb).Value);
        
        builder.HasMany(t => t.Employees)
            .WithOne()
            .HasForeignKey(e => e.TeamId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.ComplexProperty(t => t.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(NameConstant.MAX_LENGTH);
        });
        
        builder.HasOne(t => t.HeadOfTeam)
            .WithOne(e => e.ManagedTeam)
            .HasForeignKey<Employee>("managed_team_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}