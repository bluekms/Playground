using System.Data;
using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Handlers.Account;

public sealed record SignUpRule(string AccountId, string Password) : IRule;

public sealed class SignUpRuleChecker : IRuleChecker<SignUpRule>
{
    private const int MinLength = 3;
    private const int MaxLength = 20;

    private readonly AuthDbContext dbContext;

    public SignUpRuleChecker(AuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CheckAsync(SignUpRule rule, CancellationToken cancellationToken)
    {
        if (rule.AccountId.Length is < MinLength or > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(rule.AccountId));
        }

        if (rule.Password.Length is < MinLength or > MaxLength)
        {
            throw new ArgumentOutOfRangeException(nameof(rule.Password));
        }

        var exists = await dbContext.Accounts.AnyAsync(row => row.AccountId == rule.AccountId, cancellationToken);
        if (exists)
        {
            throw new DuplicateNameException(nameof(rule.AccountId));
        }
    }
}
