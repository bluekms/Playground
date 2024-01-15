using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Handlers.Decorators;

public sealed class QueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(100);

    private readonly ActivitySource activitySource;
    private readonly IQueryHandler<TQuery, TResult> handler;
    private readonly ILogger<QueryHandlerDecorator<TQuery, TResult>> logger;
    private readonly IHostEnvironment hostEnvironment;

    public QueryHandlerDecorator(
        ActivitySource activitySource,
        IQueryHandler<TQuery, TResult> handler,
        ILogger<QueryHandlerDecorator<TQuery, TResult>> logger,
        IHostEnvironment hostEnvironment)
    {
        this.activitySource = activitySource;
        this.handler = handler;
        this.logger = logger;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<TResult> QueryAsync(TQuery query, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity($"Query {typeof(TQuery).Name}");
        activity?.AddTag("Playground.Query.Type", typeof(TQuery).Name);

        var sw = Stopwatch.StartNew();
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // TODO Npgsql의 CancellationToken 핸들링 재검토 필요
            // PG는 이론적으로 실행중인 쿼리를 async하게 cancellation 할 수 있지만 Npgsql은 이로 인해 다시 커넥션을 맺으면서 DB부하가 심해진다
            // https://github.com/npgsql/npgsql/issues/1801
            // https://github.com/npgsql/npgsql/issues/348
            var result = await handler.QueryAsync(query, CancellationToken.None);
            sw.Stop();

            if (sw.Elapsed < Threshold)
            {
                LogDebug(logger, query, result, sw.Elapsed.TotalMilliseconds, null);
            }
            else
            {
                if (hostEnvironment.IsProduction())
                {
                    LogWarning(logger, typeof(TQuery), sw.Elapsed.TotalMilliseconds, null);
                }
                else
                {
                    LogWarningForDev(logger, query, result, sw.Elapsed.TotalMilliseconds, null);
                }
            }

            return result;
        }
        catch (Exception e)
        {
            if (hostEnvironment.IsProduction())
            {
                LogError(logger, typeof(TQuery), sw.Elapsed.TotalMilliseconds, e);
            }
            else
            {
                LogErrorForDev(logger, query, sw.Elapsed.TotalMilliseconds, e);
            }

            throw;
        }
    }

    private static readonly Action<ILogger, TQuery, TResult, double, Exception?> LogDebug =
        LoggerMessage.Define<TQuery, TResult, double>(
            LogLevel.Debug,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerDebug),
            "Query: {@Query}, Result: {@Result}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogWarning =
        LoggerMessage.Define<Type, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerWarning),
            "Query Too Slow. {@QueryType} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TQuery, TResult, double, Exception?> LogWarningForDev =
        LoggerMessage.Define<TQuery, TResult, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerWarning),
            "Query Too Slow. Query: {@Query}, Result: {@Result}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogError =
        LoggerMessage.Define<Type, double>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerError),
            "Query Failed! {@QueryType} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TQuery, double, Exception?> LogErrorForDev =
        LoggerMessage.Define<TQuery, double>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.QueryHandlerError),
            "Query Failed! Query: {@Query}, Elapsed: {Elapsed:0.0000}ms");
}
