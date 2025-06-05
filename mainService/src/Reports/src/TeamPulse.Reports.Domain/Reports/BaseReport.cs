using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;
using TeamPulse.SharedKernel.SharedVO;

namespace TeamPulse.Reports.Domain.Reports;

public class BaseReport : Entity<ReportId>
{
    //ef core
    private BaseReport() {}
    
    public BaseReport(
        ReportId id,
        Name name,
        Description description,
        Guid departmentId,
        Guid? teamId,
        Guid? employeeId) : base(id)
    {
        Name = name;
        Description = description;
        DepartmentId = departmentId;
        TeamId = teamId;
        EmployeeId = employeeId;
    }
    
    public Guid DepartmentId { get; protected set; }

    public Guid? TeamId { get; protected set; } = null;
    
    public Guid? EmployeeId { get; protected set; } = null;
    
    public Name Name { get; protected set; } = null!;
    
    public Description Description { get; protected set; } = null!;
    
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}