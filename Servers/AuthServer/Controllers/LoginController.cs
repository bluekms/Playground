using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Feature.Session;
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
    private readonly IConfiguration appConfig;
    private readonly IRuleChecker<LoginRule> rule;
    private readonly ICommandHandler<UpdateSessionCommand, AccountData> updateSession;
    private readonly SessionStore sessionStore;
    private readonly IQueryHandler<GetServerListQuery, List<ServerData>> getServerList;

    public LoginController(
        IConfiguration appConfig,
        IRuleChecker<LoginRule> rule,
        ICommandHandler<UpdateSessionCommand, AccountData> updateSession,
        SessionStore sessionStore,
        IQueryHandler<GetServerListQuery, List<ServerData>> getServerList)
    {
        this.appConfig = appConfig;
        this.rule = rule;
        this.updateSession = updateSession;
        this.sessionStore = sessionStore;
        this.getServerList = getServerList;
    }

    [HttpPost]
    [Route("Auth/Login")]
    [Authorize(AuthenticationSchemes = OpenAuthenticationSchemeOptions.Name)]
    public async Task<ActionResult<Result>> Login(
        [FromBody] Arguments args,
        CancellationToken cancellationToken)
    {
        await rule.CheckAsync(new(args.AccountId, args.Password, appConfig["AppSecrets:AccountSalt"]!), cancellationToken);

        var account = await updateSession.ExecuteAsync(new(args.AccountId, appConfig["AppSecrets:SessionPrefix"]!));

        var session = new SessionInfo(account.Token, account.Role);
        await sessionStore.SetAsync(session, cancellationToken);

        var worlds = await getServerList.QueryAsync(new(ServerRoles.World), cancellationToken);

        return new Result(account.Token, worlds);
    }

    public sealed record Arguments(string AccountId, string Password);

    public sealed record Result(string SessionId, List<ServerData> Worlds);
}
