using System.Data;
using System.Threading.Tasks;
using AccountServer.Models;
using CommonLibrary.Handlers;

namespace AccountServer.Handlers.Accounts
{
    public sealed record SignUpRule(string AccountId) : IRule;

    public sealed class SignUpRuleChecker : IRuleChecker<SignUpRule>
    {
        private readonly IQueryHandler<SelectAccountQuery, AccountData?> _selectAccount;

        public SignUpRuleChecker(IQueryHandler<SelectAccountQuery, AccountData?> selectAccount)
        {
            _selectAccount = selectAccount;
        }

        public async Task CheckAsync(SignUpRule rule)
        {
            var account = await _selectAccount.QueryAsync(new(rule.AccountId));
            if (account != null)
            {
                throw new DuplicateNameException(nameof(rule.AccountId));
            }
        }
    }
}