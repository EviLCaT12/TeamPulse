using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Domain.Reports;

public class BaseReport : Entity<ReportId>
{
    //ef core
    private BaseReport() {}

    //ToDo: Заменить на Create
    public BaseReport(
        ReportId id,
        Guid departmentId,
        Guid teamId,
        Name name,
        Description description) : base(id)
    {
        DepartmentId = departmentId;
        TeamId = teamId;
        Name = name;
        Description = description;
    }
    
    public Guid DepartmentId { get; protected set; }
    
    public Guid TeamId { get; protected set; }
    
    public Name Name { get; protected set; } = null!;
    
    public Description Description { get; protected set; } = null!;
    
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}