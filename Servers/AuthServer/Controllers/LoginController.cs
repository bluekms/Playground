using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Session;
using AuthServer.Handlers.World;
using AuthServer.Models;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthServer.Controllers
{
    [ApiController]
    public sealed class LoginController : ControllerBase
    {
        private readonly ILogger<SignUpController> logger;
        private readonly IRuleChecker<LoginRule> rule;
        private readonly ICommandHandler<DeleteSessionCommand> deleteSession;
        private readonly ICommandHandler<UpdateSessionCommand, AccountData> updateSession;
        private readonly ICommandHandler<AddSessionCommand> insertSession;
        private readonly IQueryHandler<GetServerListQuery, List<ServerData>> getServerList;

        public LoginController(
            ILogger<SignUpController> logger,
            IRuleChecker<LoginRule> rule,
            ICommandHandler<DeleteSessionCommand> deleteSession,
            ICommandHandler<UpdateSessionCommand, AccountData> updateSession,
            ICommandHandler<AddSessionCommand> insertSession,
            IQueryHandler<GetServerListQuery, List<ServerData>> getServerList)
        {
            this.logger = logger;
            this.rule = rule;
            this.deleteSession = deleteSession;
            this.updateSession = updateSession;
            this.insertSession = insertSession;
            this.getServerList = getServerList;
        }

        [HttpPut, Route("Auth/Login")]
        public async Task<ActionResult<Returns>> Login([FromBody] Arguments args)
        {
            await rule.CheckAsync(new(args.AccountId, args.Password));

            var sessionId = Guid.NewGuid().ToString();
            var account = await updateSession.ExecuteAsync(new(args.AccountId, sessionId));

            await deleteSession.ExecuteAsync(new(account.Token));
            await insertSession.ExecuteAsync(new(sessionId, account.Role));

            var worlds = await getServerList.QueryAsync(new(ServerRoles.World));

            return new Returns(account.Token, worlds);
        }

        public sealed record Arguments(string AccountId, string Password);

        public sealed record Returns(string SessionToken, List<ServerData> Worlds);
    }
}