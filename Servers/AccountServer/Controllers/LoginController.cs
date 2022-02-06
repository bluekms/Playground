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
        private readonly ILogger<SignUpController> _logger;
        private readonly IRuleChecker<LoginRule> _rule;
        private readonly ICommandHandler<RemoveSessionIdCommand> _removeSessionId;
        private readonly ICommandHandler<UpdateSessionIdCommand, AccountData> _updateSessionId;
        private readonly ICommandHandler<WriteSessionIdCommand> _writeSessionId;
        private readonly IQueryHandler<ListWorldsQuery, List<WorldData>> _listWorlds;

        public LoginController(
            ILogger<SignUpController> logger,
            IRuleChecker<LoginRule> rule,
            ICommandHandler<RemoveSessionIdCommand> removeSessionId,
            ICommandHandler<UpdateSessionIdCommand, AccountData> updateSessionId,
            ICommandHandler<WriteSessionIdCommand> writeSessionId,
            IQueryHandler<ListWorldsQuery, List<WorldData>> listWorlds)
        {
            _logger = logger;
            _rule = rule;
            _removeSessionId = removeSessionId;
            _updateSessionId = updateSessionId;
            _writeSessionId = writeSessionId;
            _listWorlds = listWorlds;
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
                _logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<ReturnData> HandleAsync(ArgumentData args)
        {
            await _rule.CheckAsync(new(args.AccountId, args.Password));

            var sessionId = Guid.NewGuid().ToString();
            var account = await _updateSessionId.ExecuteAsync(new(args.AccountId, sessionId));

            await _removeSessionId.ExecuteAsync(new(account.SessionId));
            await _writeSessionId.ExecuteAsync(new(sessionId, account.Authority));

            var worlds = await _listWorlds.QueryAsync(new(args.TargetWorldType));

            return new(account.SessionId, worlds);
        }

        public sealed record ArgumentData(string AccountId, string Password, string TargetWorldType);

        public sealed record ReturnData(string SessionId, List<WorldData> Worlds);
    }
}