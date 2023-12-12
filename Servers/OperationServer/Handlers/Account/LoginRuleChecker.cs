using CommonLibrary.Handlers;

namespace OperationServer.Handlers.Account;

public sealed record LoginRule(string AccountId, string Password, string Salt) : IRule;

public class LoginRuleChecker : IRuleChecker<LoginRule>
{
    public Task CheckAsync(LoginRule rule, CancellationToken cancellationToken)
    {

    }
}
