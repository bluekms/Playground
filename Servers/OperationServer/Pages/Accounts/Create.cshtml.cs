using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Mvc.Rendering;
using OperationServer.Handlers.Account;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly ICommandHandler<CreateAccountCommand> createAccount;

        public CreateModel(ICommandHandler<CreateAccountCommand> createAccount)
        {
            this.createAccount = createAccount;

            RoleList = Enum.GetValues(typeof(ResSignUp.Types.AccountRoles))
                .Cast<ResSignUp.Types.AccountRoles>()
                .Select(role => new SelectListItem
                {
                    Value = role.ToString(),
                    Text = role.ToString(),
                }).ToList();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AccountCreateVM Account { get; set; } = default!;

        public List<SelectListItem> RoleList { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await createAccount.ExecuteAsync(new(Account.AccountId, Account.Password, Account.Role));

            return RedirectToPage("./Index");
        }
    }
}
