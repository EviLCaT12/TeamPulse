using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Infrastructure.Configurations.Read;

public class EmployeeDtoConfiguration : IEntityTypeConfiguration<EmployeeDto>
{
    public void Configure(EntityTypeBuilder<EmployeeDto> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(e => e.Id);
        
        
    }
}