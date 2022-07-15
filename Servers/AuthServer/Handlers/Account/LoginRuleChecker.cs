using System;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account
{
    public sealed record LoginRule(string AccountId, string Password) : IRule;

    public sealed class LoginRuleChecker : IRuleChecker<LoginRule>
    {
        private readonly AuthDbContext dbContext;

        public LoginRuleChecker(AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CheckAsync(LoginRule rule)
        {
            var row = await dbContext.Accounts
                .Where(x => x.AccountId == rule.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                throw new NullReferenceException(nameof(rule.AccountId));
            }

            if (!rule.Password.Equals(row.Password))
            {
                throw new ArgumentException(string.Empty, nameof(rule.Password));
            }
        }
    }
}
