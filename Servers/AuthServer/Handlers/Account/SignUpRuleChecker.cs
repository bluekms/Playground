using System.Data;
using System.Threading.Tasks;
using AuthLibrary.Models;
using AuthServer.Models;
using CommonLibrary.Handlers;

namespace AuthServer.Handlers.Account
{
    public sealed record SignUpRule(string AccountId) : IRule;

    public sealed class SignUpRuleChecker : IRuleChecker<SignUpRule>
    {
        private readonly IQueryHandler<GetAccountQuery, AccountData?> getAccount;

        public SignUpRuleChecker(IQueryHandler<GetAccountQuery, AccountData?> getAccount)
        {
            this.getAccount = getAccount;
        }

        public async Task CheckAsync(SignUpRule rule)
        {
            var account = await getAccount.QueryAsync(new(rule.AccountId));
            if (account != null)
            {
                throw new DuplicateNameException(nameof(rule.AccountId));
            }
        }
    }
}