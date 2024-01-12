using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record LoginRule(string AccountId, string Password, string Salt) : IRule;

public class LoginRuleChecker : IRuleChecker<LoginRule>
{
    private ReadOnlyAuthDbContext dbContext;

    public LoginRuleChecker(ReadOnlyAuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CheckAsync(LoginRule rule, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts
            .Where(row => row.AccountId == rule.AccountId)
            .SingleAsync(cancellationToken);

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        var result = passwordHasher.VerifyHashedPassword(account, account.Password, rule.Salt + rule.Password);

        if (result is not (PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded))
        {
            throw new ArgumentException(rule.Password);
        }
    }
}
