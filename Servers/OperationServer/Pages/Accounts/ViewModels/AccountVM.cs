namespace OperationServer.Pages.Accounts.ViewModels;

public class AccountVM
{
    public required string AccountId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ResSignUp.Types.AccountRoles Role { get; set; }
}