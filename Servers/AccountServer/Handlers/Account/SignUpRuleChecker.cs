using System.Data;
using System.Threading.Tasks;
using AccountServer.Models;
using CommonLibrary.Handlers;

namespace AccountServer.Handlers.Account
{
    public sealed record SignUpRule(string AccountId) : IRule;

    public sealed class SignUpRuleChecker : IRuleChecker<SignUpRule>
    {
        private readonly IQueryHandler<GetAccountQuery, AccountData?> selectAccount;

        public SignUpRuleChecker(IQueryHandler<GetAccountQuery, AccountData?> selectAccount)
        {
            this.selectAccount = selectAccount;
        }

        public async Task CheckAsync(SignUpRule rule)
        {
            var account = await selectAccount.QueryAsync(new(rule.AccountId));
            if (account != null)
            {
                throw new DuplicateNameException(nameof(rule.AccountId));
            }
        }
    }
}