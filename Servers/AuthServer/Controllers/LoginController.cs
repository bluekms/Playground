using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Models;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Session;
using AuthServer.Handlers.World;
using AuthServer.Models;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers;

[ApiController]
public sealed class LoginController : ControllerBase
{
    private readonly IRuleChecker<LoginRule> rule;
    private readonly ICommandHandler<UpdateSessionCommand, AccountData> updateSession;
    private readonly IQueryHandler<GetServerListQuery, List<ServerData>> getServerList;

    public LoginController(
        IRuleChecker<LoginRule> rule,
        ICommandHandler<UpdateSessionCommand, AccountData> updateSession,
        IQueryHandler<GetServerListQuery, List<ServerData>> getServerList)
    {
        this.rule = rule;
        this.updateSession = updateSession;
        this.getServerList = getServerList;
    }

    [HttpPost]
    [Route("Auth/Login")]
    [Authorize(AuthenticationSchemes = OpenAuthenticationSchemeOptions.Name)]
    public async Task<ActionResult<Result>> Login([FromBody] Arguments args)
    {
        await rule.CheckAsync(new(args.AccountId, args.Password));

        var account = await updateSession.ExecuteAsync(new(args.AccountId, Guid.NewGuid().ToString()));
        var worlds = await getServerList.QueryAsync(new(ServerRoles.World));

        return new Result(account.Token, worlds);
    }

    public sealed record Arguments(string AccountId, string Password);

    public sealed record Result(string SessionId, List<ServerData> Worlds);
}