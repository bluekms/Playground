using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Feature.Session;
using AuthLibrary.Handlers;
using AuthLibrary.Models;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OperationServer.Controllers;

[ApiController]
public class SignUpController : ControllerBase
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
    [Route("Operation/SignUp")]
    [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = ApiPolicies.AdminApi)]
    public async Task<ActionResult<Result>> SignUp(
        SessionInfo session,
        [FromBody] Arguments args,
        CancellationToken cancellationToken)
    {
        await rule.CheckAsync(new(args.AccountId, args.Password), cancellationToken);

        var accountData = await addAccount.ExecuteAsync(new(
            args.AccountId,
            args.Password,
            args.Role));

        return mapper.Map<Result>(accountData);
    }

    public sealed record Arguments(string AccountId, string Password, AccountRoles Role);

    public sealed record Result(string AccountId, AccountRoles Role);
}