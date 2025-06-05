using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Infrastructure.Configurations.Write;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasConversion(id => id.Value, val => EmployeeId.Create(val).Value);

        builder.Property(e => e.WorkingTeamId)
            .HasColumnName("team_id")
            .IsRequired(false)
            .HasConversion(
                toDb => toDb != null ? toDb.Value : (Guid?)null,
                fromDb => fromDb.HasValue ? TeamId.Create(fromDb.Value).Value : null
            );

        builder.Property(e => e.WorkingDepartmentId)
            .HasColumnName("department_id")
            .IsRequired(false)
            .HasConversion(
                toDb => toDb != null ? toDb.Value : (Guid?)null,
                fromDb => fromDb.HasValue ? DepartmentId.Create(fromDb.Value).Value : null
            );

        builder.Property(e => e.IsTeamManager)
            .HasColumnName("is_team_manager");

        builder.Property(e => e.IsDepartmentManager)
            .HasColumnName("is_department_manager");
    }
}