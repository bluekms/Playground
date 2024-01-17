using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts;

// TODO PageModel을 상속받은 모든 페이지에 Authorize가 붙어있지 않으면 경고
[Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = ApiPolicies.OperationApi)]
public class IndexModel : PageModel
{
    private readonly IQueryHandler<ListAccountQuery, List<AccountVM>> listAccounts;

    public IndexModel(IQueryHandler<ListAccountQuery, List<AccountVM>> listAccounts)
    {
        this.listAccounts = listAccounts;
    }

    public IList<AccountVM> Accounts { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Accounts = await listAccounts.QueryAsync(new(), CancellationToken.None);
    }
}
