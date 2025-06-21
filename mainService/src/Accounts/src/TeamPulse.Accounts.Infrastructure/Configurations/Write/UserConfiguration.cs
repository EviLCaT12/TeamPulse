using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Accounts.Domain.Models;
using TeamPulse.Accounts.Domain.Models.AccountModels;


namespace TeamPulse.Accounts.Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();
        
        builder
            .HasOne(u => u.EmployeeAccount)
            .WithOne(p => p.User)
            .HasForeignKey<EmployeeAccount>("user_id")
            .IsRequired(false);
        
        builder
            .HasOne(u => u.AdminAccount)
            .WithOne(p => p.User)
            .HasForeignKey<AdminAccount>("user_id")
            .IsRequired(false);
    }
}