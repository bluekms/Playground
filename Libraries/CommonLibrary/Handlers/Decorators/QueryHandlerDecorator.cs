using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
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

    // TODO cancellationToken 사용하지 않도록 (데드락 검토 필요)
    public async Task<TResult> QueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity($"Query {typeof(TQuery).Name}");
        activity?.AddTag("Playground.Query.Type", typeof(TQuery).Name);

        var sw = Stopwatch.StartNew();
        try
        {
            var result = await handler.QueryAsync(query, cancellationToken);
            sw.Stop();

            if (sw.Elapsed < Threshold)
            {
                LogDebug(logger, typeof(TQuery), query, null);
            }
            else
            {
                LogWarning(logger, typeof(TQuery), query, null);
            }

            LogTrace(logger, result, null);

            return result;
        }
        catch (Exception e)
        {
            LogError(logger, typeof(TQuery), query, e);
            throw;
        }
    }

    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(200);

    private static readonly Action<ILogger, TResult, Exception?> LogTrace =
        LoggerMessage.Define<TResult>(
            LogLevel.Trace,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerTrace),
            "Query Result {@QueryResult}");

    private static readonly Action<ILogger, Type, TQuery, Exception?> LogDebug =
        LoggerMessage.Define<Type, TQuery>(
            LogLevel.Debug,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerDebug),
            "Query {QueryType} {@Query}");

    private static readonly Action<ILogger, Type, TQuery, Exception?> LogWarning =
        LoggerMessage.Define<Type, TQuery>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerWarning),
            "Query {QueryType} {@Query}");

    private static readonly Action<ILogger, Type, TQuery, Exception?> LogError =
        LoggerMessage.Define<Type, TQuery>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerError),
            "Query failed {QueryType} {@Query}");
}
