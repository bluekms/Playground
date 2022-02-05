using System;
using System.Threading.Tasks;
using AccountServer.Handlers;
using AccountServer.Handlers.Accounts;
using AccountServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountServer.Controllers
{
    [ApiController]
    public sealed class SignUpController : ControllerBase
    {
        private readonly ILogger<SignUpController> _logger;
        private readonly IRuleChecker<SignUpRule> _rule;
        private readonly ICommandHandler<InsertAccountCommand, AccountData> _insertAccount;

        public SignUpController(
            ILogger<SignUpController> logger,
            IRuleChecker<SignUpRule> rule,
            ICommandHandler<InsertAccountCommand, AccountData> insertAccount)
        {
            _logger = logger;
            _rule = rule;
            _insertAccount = insertAccount;
        }

        [HttpPost, Route("Account/SignUp")]
        public async Task<ActionResult<AuthContext.Account>> SignUp([FromBody] AuthContext.Account accountItem)
        {
            try
            {
                // TODO mapster 도입
                return await HandleAsync(new(accountItem.AccountId, accountItem.Password, accountItem.Authority));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<AuthContext.Account> HandleAsync(RequestWrapper request)
        {
            await _rule.CheckAsync(new(request.AccountId));

            var account = await _insertAccount.ExecuteAsync(new(
                request.AccountId,
                request.Password,
                "SessionId",
                request.Authority));

            // TODO mapster
            return new(
                account.AccountId,
                string.Empty,
                account.SessionId,
                account.CreatedAt,
                account.Authority);
        }
    }

    public sealed record RequestWrapper(string AccountId, string Password, string Authority);
    public sealed record ResponseWrapper(AuthContext.Account NewAccount);
}