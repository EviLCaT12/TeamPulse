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
            .WithOne()
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(d => d.Employees)
            .WithOne()
            .HasForeignKey(e => e.WorkingDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}