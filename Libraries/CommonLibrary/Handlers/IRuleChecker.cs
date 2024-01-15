namespace CommonLibrary.Handlers;

public interface IRule
{
}

public interface IRuleChecker<in TRule> : IHandlerBase
    where TRule : IRule
{
    Task CheckAsync(TRule rule, CancellationToken cancellationToken);
}

public interface IRuleChecker<in TRule, TResult> : IHandlerBase
    where TRule : IRule
{
    Task<TResult> CheckAsync(TRule rule, CancellationToken cancellationToken);
}
