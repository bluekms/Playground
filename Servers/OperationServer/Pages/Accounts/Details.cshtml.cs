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
public class DetailsModel : PageModel
{
    private readonly IQueryHandler<GetAccountQuery, AccountVM> getAccount;

    public DetailsModel(IQueryHandler<GetAccountQuery, AccountVM> getAccount)
    {
        this.getAccount = getAccount;
    }

    public AccountVM Account { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string accountId)
    {
        Account = await getAccount.QueryAsync(new(accountId), CancellationToken.None);

        return Page();
    }
}
