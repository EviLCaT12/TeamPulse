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
        

        builder.ComplexProperty(t => t.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(NameConstant.MAX_LENGTH);
        });
        
        builder.Property(t => t.DepartmentId)
            .HasColumnName("department_id")
            .IsRequired(false)
            .HasConversion(
                toDb => toDb.Value,
                fromDb => DepartmentId.Create(fromDb).Value
            );
        
        builder.HasMany(t => t.Employees)
            .WithOne()
            .HasForeignKey(e => e.WorkingTeamId)
            .OnDelete(DeleteBehavior.Restrict);
        
                
    }
}