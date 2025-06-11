using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Teams.Application.DatabaseAbstraction;
using TeamPulse.Teams.Contract.Dtos;

namespace TeamPulse.Teams.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext
{
    public IQueryable<DepartmentDto> Departments => Set<DepartmentDto>();
    
    public IQueryable<TeamDto> Teams => Set<TeamDto>();
    
    public IQueryable<EmployeeDto> Employees => Set<EmployeeDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
        
        modelBuilder.HasDefaultSchema("departments");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}