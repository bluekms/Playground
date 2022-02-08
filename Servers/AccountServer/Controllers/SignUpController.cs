using System;
using System.Threading.Tasks;
using AccountServer.Handlers.Account;
using AccountServer.Models;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountServer.Controllers
{
    [ApiController]
    public sealed class SignUpController : ControllerBase
    {
        private readonly ILogger<SignUpController> logger;
        private readonly IRuleChecker<SignUpRule> rule;
        private readonly ICommandHandler<InsertAccountCommand, AccountData> insertAccount;

        public SignUpController(
            ILogger<SignUpController> logger,
            IRuleChecker<SignUpRule> rule,
            ICommandHandler<InsertAccountCommand, AccountData> insertAccount)
        {
            this.logger = logger;
            this.rule = rule;
            this.insertAccount = insertAccount;
        }

        [HttpPost, Route("Account/SignUp")]
        public async Task<ActionResult<AccountData>> SignUp([FromBody] ArgumentData args)
        {
            try
            {
                return await HandleAsync(new(args.AccountId, args.Password, args.Authority));
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<AccountData> HandleAsync(ArgumentData args)
        {
            await rule.CheckAsync(new(args.AccountId));

            var account = await insertAccount.ExecuteAsync(new(
                args.AccountId,
                args.Password,
                args.Authority));

            return account;
        }

        public sealed record ArgumentData(string AccountId, string Password, string Authority);
    }
}