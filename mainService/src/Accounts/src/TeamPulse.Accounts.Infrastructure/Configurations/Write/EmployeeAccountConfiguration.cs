using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Accounts.Domain.Models.AccountModels;

namespace TeamPulse.Accounts.Infrastructure.Configurations.Write;

public class EmployeeAccountConfiguration : IEntityTypeConfiguration<EmployeeAccount>
{
    public void Configure(EntityTypeBuilder<EmployeeAccount> builder)
    {
        builder.ToTable("employee_accounts");
        
        builder.HasKey(ea => ea.Id);
    }
}