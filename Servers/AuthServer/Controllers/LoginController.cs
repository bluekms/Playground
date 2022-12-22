using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Handlers;
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
    private readonly ICommandHandler<DeleteSessionCommand> deleteSession;
    private readonly ICommandHandler<UpdateSessionCommand, AccountData> updateSession;
    private readonly ICommandHandler<AddSessionCommand> addSession;
    private readonly IQueryHandler<GetServerListQuery, List<ServerData>> getServerList;

    public LoginController(
        IRuleChecker<LoginRule> rule,
        ICommandHandler<DeleteSessionCommand> deleteSession,
        ICommandHandler<UpdateSessionCommand, AccountData> updateSession,
        ICommandHandler<AddSessionCommand> addSession,
        IQueryHandler<GetServerListQuery, List<ServerData>> getServerList)
    {
        this.rule = rule;
        this.deleteSession = deleteSession;
        this.updateSession = updateSession;
        this.addSession = addSession;
        this.getServerList = getServerList;
    }

    [HttpPost]
    [Route("Auth/Login")]
    [Authorize(AuthenticationSchemes = OpenAuthenticationSchemeOptions.Name)]
    public async Task<ActionResult<Result>> Login([FromBody] Arguments args)
    {
        await rule.CheckAsync(new(args.AccountId, args.Password));

        var sessionId = Guid.NewGuid().ToString();
        var account = await updateSession.ExecuteAsync(new(args.AccountId, sessionId));

        await deleteSession.ExecuteAsync(new(account.Token));
        await addSession.ExecuteAsync(new(sessionId, account.Role));

        var worlds = await getServerList.QueryAsync(new(ServerRoles.World));

        return new Result(account.Token, worlds);
    }

    public sealed record Arguments(string AccountId, string Password);

    public sealed record Result(string SessionId, List<ServerData> Worlds);
}