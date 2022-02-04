using System.Threading.Tasks;

namespace AccountServer.Features
{
    public interface IQuery
    {
    }

    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery
    {
        Task<TResult> QueryAsync(TQuery query);
    }
}