using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthDb;
using CommonLibrary.Handlers;
using Microsoft.EntityFrameworkCore;

namespace AccountServer.Handlers.Accounts
{
    public sealed record LoginRule(string AccountId, string Password) : IRule;

    public sealed class LoginRuleChecker : IRuleChecker<LoginRule>
    {
        private readonly AuthContext _context;

        public LoginRuleChecker(AuthContext context)
        {
            _context = context;
        }

        public async Task CheckAsync(LoginRule rule)
        {
            var row = await _context.Accounts
                .Where(x => x.AccountId == rule.AccountId)
                .SingleOrDefaultAsync();

            if (row == null)
            {
                throw new NullReferenceException(nameof(rule.AccountId));
            }

            if (!row.Password.Equals(rule.Password))
            {
                throw new ArgumentException("", nameof(rule.Password));
            }
        }
    }
}
