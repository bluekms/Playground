using System.Threading.Tasks;

namespace AccountServer.Handlers
{
    public interface IQuery
    {
    }

    public interface IQueryHandler<in TQuery, TResult> : IHandlerBase
        where TQuery : IQuery
    {
        Task<TResult> QueryAsync(TQuery query);
    }
}