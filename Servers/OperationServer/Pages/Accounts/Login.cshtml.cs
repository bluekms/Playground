using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperationServer.Pages.Accounts.ViewModels;

namespace OperationServer.Pages.Accounts;

public class LoginModel : PageModel
{
    [BindProperty]
    public CredentialVM Credential { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
