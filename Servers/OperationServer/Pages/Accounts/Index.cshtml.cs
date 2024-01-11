using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts
{
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
}
