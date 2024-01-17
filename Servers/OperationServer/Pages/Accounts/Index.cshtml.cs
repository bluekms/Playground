using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts;

// TODO PageModel을 상속받은 모든 페이지에 Authorize가 붙어있지 않으면 경고
// TODO 여기서 AuthServer를 Using 하고 있으면 경고
// TODO 컨트롤러와 페이지에 DI되어야 할거 잘못 연결하면 실패하는 테스트 제작
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
