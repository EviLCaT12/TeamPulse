namespace TeamPulse.Accounts.Domain.Models.AccountModels;

public class EmployeeAccount
{ 
    //ef core
    private EmployeeAccount() {}

    public EmployeeAccount(User user, Guid employeeId)
    {
        Id = Guid.NewGuid();
        User = user;
        EmployeeId = employeeId;
    }
    
    public const string Employee = nameof(Employee);
    
    public Guid Id { get; set; }
    
    public User User { get; set; }
    
    public Guid EmployeeId { get; set; }
}