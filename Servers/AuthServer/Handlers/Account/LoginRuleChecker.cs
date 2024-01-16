using AuthDb;
using CommonLibrary;
using CommonLibrary.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account;

public sealed record LoginRule(string AccountId, string Password) : IRule;

public sealed record LoginRuleResult(bool RehashNeeded);

public sealed class LoginRuleChecker : IRuleChecker<LoginRule, LoginRuleResult>
{
    private readonly ITimeService timeService;
    private readonly ReadOnlyAuthDbContext dbContext;

    public LoginRuleChecker(
        ITimeService timeService,
        ReadOnlyAuthDbContext dbContext)
    {
        this.timeService = timeService;
        this.dbContext = dbContext;
    }

    public async Task<LoginRuleResult> CheckAsync(LoginRule rule, CancellationToken cancellationToken)
    {
        var accountRow = await dbContext.Accounts
            .Where(x => x.AccountId == rule.AccountId)
            .SingleAsync(cancellationToken);

        var passwordRow = await dbContext.Passwords
            .Where(x => x.AccountId == rule.AccountId)
            .SingleAsync(cancellationToken);

        var passwordHasher = new PasswordHasher<AuthDb.Account>();
        var result = passwordHasher.VerifyHashedPassword(accountRow, passwordRow.AccountPassword, rule.Password);

        switch (result)
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
                return new(true);

            case PasswordVerificationResult.Success:
                {
                    if (passwordRow.UpdatedAt < timeService.Now.AddDays(-30))
                    {
                        return new(true);
                    }
                    else
                    {
                        return new(false);
                    }
                }

            default:
            case PasswordVerificationResult.Failed:
                throw new ArgumentException(rule.Password);
        }
    }
}
