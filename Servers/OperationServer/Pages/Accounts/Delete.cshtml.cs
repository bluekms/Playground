using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly IQueryHandler<GetAccountQuery, AccountVM> getAccount;
        private readonly ICommandHandler<DeleteAccountCommand> deleteAccount;

        public DeleteModel(
            IQueryHandler<GetAccountQuery, AccountVM> getAccount,
            ICommandHandler<DeleteAccountCommand> deleteAccount)
        {
            this.getAccount = getAccount;
            this.deleteAccount = deleteAccount;
        }

        [BindProperty]
        public AccountVM Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string accountId)
        {
            Account = await getAccount.QueryAsync(new(accountId), CancellationToken.None);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string accountId)
        {
            await deleteAccount.ExecuteAsync(new(accountId));

            return RedirectToPage("./Index");
        }
    }
}
