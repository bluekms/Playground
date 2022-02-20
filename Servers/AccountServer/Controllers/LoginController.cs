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
        private readonly ICommandHandler<DeleteSessionIdCommand> deleteSessionId;
        private readonly ICommandHandler<UpdateSessionIdCommand, AccountData> updateSessionId;
        private readonly ICommandHandler<InsertSessionIdCommand> insertSessionId;
        private readonly IQueryHandler<GetWorldListQuery, List<WorldData>> getWorldList;

        public LoginController(
            ILogger<SignUpController> logger,
            IRuleChecker<LoginRule> rule,
            ICommandHandler<DeleteSessionIdCommand> deleteSessionId,
            ICommandHandler<UpdateSessionIdCommand, AccountData> updateSessionId,
            ICommandHandler<InsertSessionIdCommand> insertSessionId,
            IQueryHandler<GetWorldListQuery, List<WorldData>> getWorldList)
        {
            this.logger = logger;
            this.rule = rule;
            this.deleteSessionId = deleteSessionId;
            this.updateSessionId = updateSessionId;
            this.insertSessionId = insertSessionId;
            this.getWorldList = getWorldList;
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

            await deleteSessionId.ExecuteAsync(new(account.SessionId));
            await insertSessionId.ExecuteAsync(new(sessionId, account.UserRole));

            var worlds = await getWorldList.QueryAsync(new(args.TargetWorldType));

            return new(account.SessionId, worlds);
        }

        public sealed record ArgumentData(string AccountId, string Password, string TargetWorldType);

        public sealed record ReturnData(string SessionId, List<WorldData> Worlds);
    }
}