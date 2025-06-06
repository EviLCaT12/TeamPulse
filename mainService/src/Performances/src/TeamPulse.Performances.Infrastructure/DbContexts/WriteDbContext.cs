using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TeamPulse.Performances.Domain.Entities;
using TeamPulse.Performances.Domain.Entities.SkillGrade;

namespace TeamPulse.Performances.Infrastructure.DbContexts;

public class WriteDbContext(string connectionString) : DbContext
{
    public DbSet<BaseSkillGrade> SkillGrades => Set<BaseSkillGrade>();
    
    public DbSet<GroupOfSkills> GroupOfSkills => Set<GroupOfSkills>();
    
    public DbSet<GroupSkill> GroupSkills => Set<GroupSkill>();
    
    public DbSet<RecordSkill> RecordSkills => Set<RecordSkill>();
    
    public DbSet<Skill> Skills => Set<Skill>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
        
        modelBuilder.HasDefaultSchema("performances");
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}