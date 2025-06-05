using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Performances.Application.DatabaseAbstraction;
using TeamPulse.Performances.Contract.Dtos;

namespace TeamPulse.Performances.Infrastructure.DbContexts;

public class ReadDbContext(string connectionString) : DbContext, IReadDbContext
{
    public IQueryable<GroupOfSkillsDto> GroupOfSkills => Set<GroupOfSkillsDto>();
    
    public IQueryable<SkillGradeDto> SkillGrades => Set<SkillGradeDto>();
    
    public IQueryable<GroupSkillDto> GroupSkills => Set<GroupSkillDto>();
    
    public IQueryable<SkillDto> Skills => Set<SkillDto>();
    
    public IQueryable<RecordSkillDto> RecordSkills => Set<RecordSkillDto>();

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
        
        modelBuilder.HasDefaultSchema("performances");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}
