using System.Data;
using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace OperationServer.Handlers.Account;

public sealed record CreateAccountRule(string AccountId, string Password) : IRule;

public class CreateAccountRuleChecker : IRuleChecker<CreateAccountRule>
{
    private const int MinLength = 4;
    private const int MaxLength = 20;

    private readonly ReadOnlyAuthDbContext dbContext;

    public CreateAccountRuleChecker(ReadOnlyAuthDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CheckAsync(CreateAccountRule rule, CancellationToken cancellationToken)
    {
        if (rule.AccountId.Length is < MinLength or > MaxLength)
        {
            throw new ArgumentOutOfRangeException(rule.AccountId);
        }

        if (rule.Password.Length is < MinLength or > MaxLength)
        {
            throw new ArgumentOutOfRangeException(rule.Password);
        }

        var exists = await dbContext.Accounts.AnyAsync(row => row.AccountId == rule.AccountId, cancellationToken);
        if (exists)
        {
            throw new DuplicateNameException(nameof(rule.AccountId));
        }
    }
}
