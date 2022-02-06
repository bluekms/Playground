using System;
using System.Threading.Tasks;
using AccountServer.Handlers.Account;
using AccountServer.Handlers.Session;
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

        public LoginController(
            ILogger<SignUpController> logger,
            IRuleChecker<LoginRule> rule,
            ICommandHandler<RemoveSessionIdCommand> removeSessionId,
            ICommandHandler<UpdateSessionIdCommand, AccountData> updateSessionId,
            ICommandHandler<WriteSessionIdCommand> writeSessionId)
        {
            _logger = logger;
            _rule = rule;
            _removeSessionId = removeSessionId;
            _updateSessionId = updateSessionId;
            _writeSessionId = writeSessionId;
        }

        [HttpPut, Route("Account/Login")]
        public async Task<ActionResult<AccountData>> Login([FromBody] Argument request)
        {
            try
            {
                return await HandleAsync(new(request.AccountId, request.Password));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<AccountData> HandleAsync(Argument request)
        {
            await _rule.CheckAsync(new(request.AccountId, request.Password));

            var sessionId = Guid.NewGuid().ToString();
            var account = await _updateSessionId.ExecuteAsync(new(request.AccountId, sessionId));

            await _removeSessionId.ExecuteAsync(new(account.SessionId));
            await _writeSessionId.ExecuteAsync(new(sessionId, account.Authority));

            return account;
        }

        public sealed record Argument(string AccountId, string Password);
    }
}