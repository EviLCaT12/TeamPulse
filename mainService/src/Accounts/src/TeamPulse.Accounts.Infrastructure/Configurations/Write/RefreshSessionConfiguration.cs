using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Accounts.Domain.Models;

namespace TeamPulse.Accounts.Infrastructure.Configurations.Write;

public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
{
    public void Configure(EntityTypeBuilder<RefreshSession> builder)
    {
        builder.ToTable("refresh_sessions");
        
        builder.HasKey(x => x.Id);

        builder
            .HasOne(rs => rs.User)
            .WithMany()
            .HasForeignKey(rs => rs.UserId);
    }
}