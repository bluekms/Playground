using System;
using System.Threading.Tasks;
using AccountServer.Handlers.Account;
using AccountServer.Models;
using CommonLibrary.Handlers;
using CommonLibrary.Models;
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

        [HttpPost, Route("Auth/SignUp")]
        public async Task<ActionResult<AccountData>> SignUp([FromBody] Arguments args)
        {
            await rule.CheckAsync(new(args.AccountId));

            return await insertAccount.ExecuteAsync(new(
                args.AccountId,
                args.Password,
                args.Role));
        }

        public sealed record Arguments(string AccountId, string Password, UserRoles Role);
    }
}