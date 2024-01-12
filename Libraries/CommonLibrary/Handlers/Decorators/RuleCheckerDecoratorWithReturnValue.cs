using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Handlers.Decorators;

public class RuleCheckerDecorator<TRule, TResult> : IRuleChecker<TRule, TResult>
    where TRule : IRule
{
    private static readonly TimeSpan Threshold = TimeSpan.FromMilliseconds(100);

    private readonly ActivitySource activitySource;
    private readonly IRuleChecker<TRule, TResult> checker;
    private readonly ILogger<RuleCheckerDecorator<TRule, TResult>> logger;
    private readonly IHostEnvironment hostEnvironment;

    public RuleCheckerDecorator(
        ActivitySource activitySource,
        IRuleChecker<TRule, TResult> checker,
        ILogger<RuleCheckerDecorator<TRule, TResult>> logger,
        IHostEnvironment hostEnvironment)
    {
        this.activitySource = activitySource;
        this.checker = checker;
        this.logger = logger;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<TResult> CheckAsync(TRule rule, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity($"Rule {typeof(TRule).Name}");
        activity?.AddTag("Playground.Rule.Type", typeof(TRule).Name);

        var sw = Stopwatch.StartNew();
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            // TODO Npgsql의 CancellationToken 핸들링 재검토 필요
            // PG는 이론적으로 실행중인 쿼리를 async하게 cancellation 할 수 있지만 Npgsql은 이로 인해 다시 커넥션을 맺으면서 DB부하가 심해진다
            // https://github.com/npgsql/npgsql/issues/1801
            // https://github.com/npgsql/npgsql/issues/348
            var result = await checker.CheckAsync(rule, CancellationToken.None);
            sw.Stop();

            if (sw.Elapsed < Threshold)
            {
                LogInformation(logger, rule, result, sw.Elapsed.TotalMilliseconds, null);
            }
            else
            {
                if (hostEnvironment.IsProduction())
                {
                    LogWarning(logger, typeof(TRule), sw.Elapsed.TotalMilliseconds, null);
                }
                else
                {
                    LogWarningForDev(logger, rule, result, sw.Elapsed.TotalMilliseconds, null);
                }
            }

            return result;
        }
        catch (Exception e)
        {
            if (hostEnvironment.IsProduction())
            {
                LogError(logger, typeof(TRule), sw.Elapsed.TotalMilliseconds, e);
            }
            else
            {
                LogErrorForDev(logger, rule, sw.Elapsed.TotalMilliseconds, e);
            }

            throw;
        }
    }

    private static readonly Action<ILogger, TRule, TResult, double, Exception?> LogInformation =
        LoggerMessage.Define<TRule, TResult, double>(
            LogLevel.Information,
            EventIdFactory.Create(ReservedLogEventId.RuleCheckerInformation),
            "Rule: {@Rule}, Result: {@Result}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogWarning =
        LoggerMessage.Define<Type, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.RuleCheckerWarning),
            "Rule Too Slow. {@Rule} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TRule, TResult, double, Exception?> LogWarningForDev =
        LoggerMessage.Define<TRule, TResult, double>(
            LogLevel.Warning,
            EventIdFactory.Create(ReservedLogEventId.RuleCheckerWarning),
            "Rule Too Slow. Rule: {@Rule}, Result: {@Result}, Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, Type, double, Exception?> LogError =
        LoggerMessage.Define<Type, double>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.RuleCheckerError),
            "Rule Failed! {@RuleType} Elapsed: {Elapsed:0.0000}ms");

    private static readonly Action<ILogger, TRule, double, Exception?> LogErrorForDev =
        LoggerMessage.Define<TRule, double>(
            LogLevel.Error,
            EventIdFactory.Create(ReservedLogEventId.RuleCheckerError),
            "Rule Failed! Rule: {@Rule}, Elapsed: {Elapsed:0.0000}ms");
}
