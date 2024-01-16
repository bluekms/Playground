using AuthLibrary.Extensions.Authentication;
using AuthServer.Handlers.Account;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public sealed class SignUpController : ControllerBase
{
    private readonly IRuleChecker<SignUpRule> rule;
    private readonly ICommandHandler<SignUpCommand> signUp;

    public SignUpController(
        IRuleChecker<SignUpRule> rule,
        ICommandHandler<SignUpCommand> signUp)
    {
        this.rule = rule;
        this.signUp = signUp;
    }

    [HttpPost]
    [Route("Auth/SignUp")]
    [Authorize(AuthenticationSchemes = OpenAuthenticationSchemeOptions.Name)]
    public async Task<Result> SignUp(
        [FromBody] Arguments args,
        CancellationToken cancellationToken)
    {
        await rule.CheckAsync(new(args.AccountId, args.Password), cancellationToken);

        await signUp.ExecuteAsync(new(args.AccountId, args.Password, ResSignUp.Types.AccountRoles.User));

        return new(args.AccountId, ResSignUp.Types.AccountRoles.User);
    }

    public sealed record Arguments(string AccountId, string Password);

    public sealed record Result(string AccountId, ResSignUp.Types.AccountRoles Role);
}
