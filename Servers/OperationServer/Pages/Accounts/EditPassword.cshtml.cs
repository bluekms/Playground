using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts;

[Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = ApiPolicies.OperationApi)]
public class EditModel : PageModel
{
    private readonly IQueryHandler<GetAccountQuery, AccountUpdatePasswordVM> getAccount;
    private readonly ICommandHandler<UpdateAccountPasswordCommand> updateAccountRole;

    public EditModel(
        IQueryHandler<GetAccountQuery, AccountUpdatePasswordVM> getAccount,
        ICommandHandler<UpdateAccountPasswordCommand> updateAccountRole)
    {
        this.getAccount = getAccount;
        this.updateAccountRole = updateAccountRole;
    }

    [BindProperty]
    public AccountUpdatePasswordVM Account { get; set; } = default!;

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

        await updateAccountRole.ExecuteAsync(new(Account.AccountId, Account.Password));

        return RedirectToPage("./Index");
    }
}
