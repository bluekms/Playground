using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account
{
    public sealed record LoginRule(string AccountId, string Password, string Salt) : IRule;

    public sealed class LoginRuleChecker : IRuleChecker<LoginRule>
    {
        private readonly ReadOnlyAuthDbContext dbContext;

        public LoginRuleChecker(ReadOnlyAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CheckAsync(LoginRule rule, CancellationToken cancellationToken)
        {
            var row = await dbContext.Accounts
                .Where(x => x.AccountId == rule.AccountId)
                .SingleOrDefaultAsync(cancellationToken);

            if (row == null)
            {
                throw new NullReferenceException(nameof(rule.AccountId));
            }

            var passwordHasher = new PasswordHasher<AuthDb.Account>();
            var result = passwordHasher.VerifyHashedPassword(row, row.Password, rule.Salt + rule.Password);

            if (result is not (PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded))
            {
                throw new ArgumentException(string.Empty, nameof(rule.Password));
            }
        }
    }
}
