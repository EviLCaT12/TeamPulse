using CSharpFunctionalExtensions;
using TeamPulse.SharedKernel.Errors;

namespace TeamPulse.Reports.Domain;

public class ReportId : ValueObject 
{
    //ef core
    private ReportId() { }
    
    private ReportId(Guid value) => Value = value;
    
    public Guid Value { get; }

    public static Result<ReportId, Error> Create(Guid reportId)
    {
        if (reportId == Guid.Empty)
            return Errors.General.ValueIsRequired("Report ID cannot be empty.");

        return new ReportId(reportId);
    }
    
    public static ReportId NewId() => new ReportId(Guid.NewGuid());
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
}