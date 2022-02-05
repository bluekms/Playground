using System;
using System.Threading.Tasks;
using AccountServer.Handlers.Accounts;
using AccountServer.Models;
using AuthDb;
using CommonLibrary.Handlers;
using MapsterMapper;
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
        public async Task<ActionResult<AccountData>> SignUp([FromBody] Argument request)
        {
            try
            {
                return await HandleAsync(new(request.AccountId, request.Password, request.Authority));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<AccountData> HandleAsync(Argument request)
        {
            await _rule.CheckAsync(new(request.AccountId));

            return await _insertAccount.ExecuteAsync(new(
                request.AccountId,
                request.Password,
                request.Authority));
        }

        public sealed record Argument(string AccountId, string Password, string Authority);
    }
}