using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Handlers.Decorators;

public sealed class QueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    private readonly IQueryHandler<TQuery, TResult> handler;
    private readonly ILogger<QueryHandlerDecorator<TQuery, TResult>> logger;
    private readonly ActivitySource activitySource;

    public QueryHandlerDecorator(
        IQueryHandler<TQuery, TResult> handler,
        ILogger<QueryHandlerDecorator<TQuery, TResult>> logger,
        ActivitySource activitySource)
    {
        this.handler = handler;
        this.logger = logger;
        this.activitySource = activitySource;
    }

    public async Task<TResult> QueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        try
        {
            using var activity = activitySource.StartActivity($"Query {typeof(TQuery).Name}");
            activity?.AddTag("Playground.Query.Type", typeof(TQuery).Name);

            var result = await handler.QueryAsync(query, cancellationToken);
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