namespace TeamPulse.Accounts.Domain.Models.AccountModels;

public class EmployeeAccount
{ 
    //ef core
    private EmployeeAccount() {}

    public EmployeeAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
    }
    
    public const string Employee = nameof(Employee);
    
    public Guid Id { get; set; }
    
    public User User { get; set; }
}