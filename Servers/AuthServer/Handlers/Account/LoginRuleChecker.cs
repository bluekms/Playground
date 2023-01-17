using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account
{
    public sealed record LoginRule(string AccountId, string Password) : IRule;

    public sealed class LoginRuleChecker : IRuleChecker<LoginRule>
    {
        private const string Salt = "Playground.bluekms";

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

            var passwordHasher = new PasswordHasher<AuthDb.Account>();
            var account = new AuthDb.Account();
            var password = Salt + rule.Password;
            var hashedPassword = passwordHasher.HashPassword(account, password);
            var result = passwordHasher.VerifyHashedPassword(account, hashedPassword, password);

            if (result is not (PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded))
            {
                throw new ArgumentException(string.Empty, nameof(rule.Password));
            }
        }
    }
}
