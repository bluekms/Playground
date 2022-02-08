using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountServer.Handlers.Account;
using AccountServer.Handlers.Session;
using AccountServer.Handlers.World;
using AccountServer.Models;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountServer.Controllers
{
    [ApiController]
    public sealed class LoginController : ControllerBase
    {
        private readonly ILogger<SignUpController> logger;
        private readonly IRuleChecker<LoginRule> rule;
        private readonly ICommandHandler<RemoveSessionIdCommand> removeSessionId;
        private readonly ICommandHandler<UpdateSessionIdCommand, AccountData> updateSessionId;
        private readonly ICommandHandler<WriteSessionIdCommand> writeSessionId;
        private readonly IQueryHandler<ListWorldsQuery, List<WorldData>> listWorlds;

        public LoginController(
            ILogger<SignUpController> logger,
            IRuleChecker<LoginRule> rule,
            ICommandHandler<RemoveSessionIdCommand> removeSessionId,
            ICommandHandler<UpdateSessionIdCommand, AccountData> updateSessionId,
            ICommandHandler<WriteSessionIdCommand> writeSessionId,
            IQueryHandler<ListWorldsQuery, List<WorldData>> listWorlds)
        {
            this.logger = logger;
            this.rule = rule;
            this.removeSessionId = removeSessionId;
            this.updateSessionId = updateSessionId;
            this.writeSessionId = writeSessionId;
            this.listWorlds = listWorlds;
        }

        [HttpPut, Route("Account/Login")]
        public async Task<ActionResult<ReturnData>> Login([FromBody] ArgumentData args)
        {
            try
            {
                return await HandleAsync(new(args.AccountId, args.Password, args.TargetWorldType));
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<ReturnData> HandleAsync(ArgumentData args)
        {
            await rule.CheckAsync(new(args.AccountId, args.Password));

            var sessionId = Guid.NewGuid().ToString();
            var account = await updateSessionId.ExecuteAsync(new(args.AccountId, sessionId));

            await removeSessionId.ExecuteAsync(new(account.SessionId));
            await writeSessionId.ExecuteAsync(new(sessionId, account.Authority));

            var worlds = await listWorlds.QueryAsync(new(args.TargetWorldType));

            return new(account.SessionId, worlds);
        }

        public sealed record ArgumentData(string AccountId, string Password, string TargetWorldType);

        public sealed record ReturnData(string SessionId, List<WorldData> Worlds);
    }
}