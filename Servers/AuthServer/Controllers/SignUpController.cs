using AuthLibrary.Extensions.Authentication;
using AuthServer.Handlers.Account;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public sealed class SignUpController : ControllerBase
{
    // TODO 유효성 검사 가능하고 nullable 체크하도록 수정하자
    private readonly IConfiguration appConfig;
    private readonly IRuleChecker<SignUpRule> rule;
    private readonly ICommandHandler<SignUpCommand> signUp;

    public SignUpController(
        IConfiguration appConfig,
        IRuleChecker<SignUpRule> rule,
        ICommandHandler<SignUpCommand> signUp)
    {
        this.appConfig = appConfig;
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

        await signUp.ExecuteAsync(new(
            appConfig["AppSecrets:AccountSalt"]!,
            args.AccountId,
            args.Password,
            ResSignUp.Types.AccountRoles.User));

        return new(args.AccountId, ResSignUp.Types.AccountRoles.User);
    }

    public sealed record Arguments(string AccountId, string Password);

    public sealed record Result(string AccountId, ResSignUp.Types.AccountRoles Role);
}
