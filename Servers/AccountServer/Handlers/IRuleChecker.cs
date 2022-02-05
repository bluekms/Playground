using System.Threading.Tasks;

namespace AccountServer.Handlers
{
    public interface IRule
    {
    }

    public interface IRuleChecker<in TRule>
        where TRule : IRule
    {
        Task CheckAsync(TRule rule);
    }
}