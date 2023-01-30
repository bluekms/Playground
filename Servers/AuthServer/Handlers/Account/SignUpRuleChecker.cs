using System.Data;
using AuthLibrary.Models;
using CommonLibrary.Handlers;

namespace AuthServer.Handlers.Account
{
    public sealed record SignUpRule(string AccountId, string Password) : IRule;

    public sealed class SignUpRuleChecker : IRuleChecker<SignUpRule>
    {
        private const int MinLength = 3;
        private const int MaxLength = 20;

        private readonly IQueryHandler<GetAccountQuery, AccountData?> getAccount;

        public SignUpRuleChecker(IQueryHandler<GetAccountQuery, AccountData?> getAccount)
        {
            this.getAccount = getAccount;
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

            var account = await getAccount.QueryAsync(new(rule.AccountId), cancellationToken);
            if (account != null)
            {
                throw new DuplicateNameException(nameof(rule.AccountId));
            }
        }
    }
}