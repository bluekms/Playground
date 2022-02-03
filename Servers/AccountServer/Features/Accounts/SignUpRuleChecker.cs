using System.Data;
using System.Threading.Tasks;
using AccountServer.Models;

namespace AccountServer.Features.Accounts
{
    public sealed record SignUpRule(string AccountId) : IRule;

    public sealed class SignUpRuleChecker : IRuleChecker<SignUpRule>
    {
        private readonly IQueryHandler<GetAccountQuery, AccountData?> _getAccount;

        public SignUpRuleChecker(IQueryHandler<GetAccountQuery, AccountData?> getAccount)
        {
            _getAccount = getAccount;
        }

        public async Task CheckAsync(SignUpRule rule)
        {
            var account = await _getAccount.QueryAsync(new(rule.AccountId));
            if (account != null)
            {
                throw new DuplicateNameException(nameof(rule.AccountId));
            }
        }
    }
}