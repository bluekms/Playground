using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Feature.Session;
using AuthLibrary.Models;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Session;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginRule = OperationServer.Handlers.Account.LoginRule;
using LoginRuleResult = OperationServer.Handlers.Account.LoginRuleResult;

namespace OperationServer.Pages.Accounts;

[Authorize(AuthenticationSchemes = OpenAuthenticationSchemeOptions.Name)]
public class LoginModel : PageModel
{
    [BindProperty]
    public required string AccountId { get; set; }

    [BindProperty]
    public required string Password { get; set; }

    private readonly IRuleChecker<LoginRule, LoginRuleResult> loginRuleChecker;
    private readonly ICommandHandler<UpdatePasswordCommand> updatePassword;
    private readonly ICommandHandler<UpdateSessionCommand, AccountData> updateSession;
    private readonly SessionStore sessionStore;

    public LoginModel(
        IRuleChecker<LoginRule, LoginRuleResult> loginRuleChecker,
        ICommandHandler<UpdatePasswordCommand> updatePassword,
        ICommandHandler<UpdateSessionCommand, AccountData> updateSession,
        SessionStore sessionStore)
    {
        this.loginRuleChecker = loginRuleChecker;
        this.updatePassword = updatePassword;
        this.updateSession = updateSession;
        this.sessionStore = sessionStore;
    }

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

        var checkResult = await loginRuleChecker.CheckAsync(new(AccountId, Password), CancellationToken.None);
        if (checkResult.RehashNeeded)
        {
            await updatePassword.ExecuteAsync(new(AccountId, Password));
        }

        var account = await updateSession.ExecuteAsync(new(AccountId));

        var session = new SessionInfo(account.Token, account.Role);
        await sessionStore.SetAsync(session, CancellationToken.None);

        return RedirectToPage("./Index");
    }
}
