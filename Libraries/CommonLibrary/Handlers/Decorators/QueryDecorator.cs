using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Handlers.Decorators;

internal sealed class QueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    private readonly IQueryHandler<TQuery, TResult> handler;
    private readonly ILogger<QueryDecorator<TQuery, TResult>> logger;
    private readonly ActivitySource activitySource;

    public QueryDecorator(
        IQueryHandler<TQuery, TResult> handler,
        ILogger<QueryDecorator<TQuery, TResult>> logger, 
        ActivitySource activitySource)
    {
        this.handler = handler;
        this.logger = logger;
        this.activitySource = activitySource;
    }

    public async Task<TResult> QueryAsync(TQuery query)
    {
        try
        {
            using var activity = activitySource.StartActivity($"Query {typeof(TQuery).Name}");
            activity?.AddTag("Playground.Query.Type", typeof(TQuery).Name);

            var result = await handler.QueryAsync(query);
            logger.LogDebug("Query {QueryType} {@Query}", typeof(TQuery).Name, query);
            logger.LogTrace("Query Result {@QueryResult}", result);

            return result;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Query failed {QueryType} {@Query}", typeof(TQuery).Name, query);
            throw;
        }
    }
}