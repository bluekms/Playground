namespace OperationServer.Pages.Accounts.ViewModels;

public class AccountUpdateRoleVM
{
    public required string AccountId { get; set; }
    public ResSignUp.Types.AccountRoles Role { get; set; }
}