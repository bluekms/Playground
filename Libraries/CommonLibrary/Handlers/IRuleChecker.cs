using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public interface IRule
    {
    }

    public interface IRuleChecker<in TRule> : IHandlerBase
        where TRule : IRule
    {
        Task CheckAsync(TRule rule, CancellationToken cancellationToken);
    }
}