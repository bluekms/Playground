using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts;

[Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = ApiPolicies.OperationApi)]
public class EditRoleModel : PageModel
{
    private readonly IQueryHandler<GetAccountQuery, AccountUpdateRoleVM> getAccount;
    private readonly ICommandHandler<UpdateAccountRoleCommand> updateAccountRole;

    public EditRoleModel(
        IQueryHandler<GetAccountQuery, AccountUpdateRoleVM> getAccount,
        ICommandHandler<UpdateAccountRoleCommand> updateAccountRole)
    {
        this.getAccount = getAccount;
        this.updateAccountRole = updateAccountRole;

        RoleList = Enum.GetValues(typeof(ResSignUp.Types.AccountRoles))
            .Cast<ResSignUp.Types.AccountRoles>()
            .Select(role => new SelectListItem
            {
                Value = role.ToString(),
                Text = role.ToString(),
            }).ToList();
    }

    [BindProperty]
    public AccountUpdateRoleVM Account { get; set; } = default!;

    public List<SelectListItem> RoleList { get; set; }

    public async Task<IActionResult> OnGetAsync(string accountId)
    {
        Account = await getAccount.QueryAsync(new(accountId), CancellationToken.None);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await updateAccountRole.ExecuteAsync(new(Account.AccountId, Account.Role));

        return RedirectToPage("./Index");
    }
}
