using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Models;
using AuthServer.Handlers.Account;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public sealed class SignUpController : ControllerBase
{
    private readonly IRuleChecker<SignUpRule> rule;
    private readonly ICommandHandler<AddAccountCommand, AccountData> addAccount;
    private readonly IMapper mapper;

    public SignUpController(
        IRuleChecker<SignUpRule> rule,
        ICommandHandler<AddAccountCommand, AccountData> addAccount,
        IMapper mapper)
    {
        this.rule = rule;
        this.addAccount = addAccount;
        this.mapper = mapper;
    }

    [HttpPost]
    [Route("Auth/SignUp")]
    [Authorize(AuthenticationSchemes = OpenAuthenticationSchemeOptions.Name)]
    public async Task<ActionResult<Result>> SignUp([FromBody] Arguments args)
    {
        await rule.CheckAsync(new(args.AccountId, args.Password));

        var accountData = await addAccount.ExecuteAsync(new(
            args.AccountId,
            args.Password,
            AccountRoles.User));

        return mapper.Map<Result>(accountData);
    }

    public sealed record Arguments(string AccountId, string Password);

    public sealed record Result(string AccountId, AccountRoles Role);
}