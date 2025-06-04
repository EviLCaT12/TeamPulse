using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.SharedKernel.Constants;
using TeamPulse.Teams.Domain.Entities;
using TeamPulse.Teams.Domain.VO.Ids;

namespace TeamPulse.Teams.Infrastructure.Configurations.Write;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .HasColumnName("id")
            .HasConversion(
                idToDb => idToDb.Value,
                idFromDb => DepartmentId.Create(idFromDb).Value);

        builder.ComplexProperty(d => d.Name, nb =>
        {
            nb.Property(n => n.Value)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(NameConstant.MAX_LENGTH);
        });
        
        builder.HasMany(d => d.Teams)
            .WithOne(t => t.Department)
            .HasForeignKey("department_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(d => d.HeadOfDepartment)
            .WithOne(e => e.ManagedDepartment)
            .HasForeignKey("managed_department_id")
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        builder.HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey("department_id")
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}