using Microsoft.Extensions.Logging;

namespace CommonLibrary;

/// <summary>
/// Trace, Debug, Information, Warning, Error, Critical, None : 1의 자리가 LogLevel과 같도록
/// CommonLibrary : 1000 ~ 1999
/// </summary>
public enum ReservedLogEventId
{
    QueryHandlerTrace = 1010,
    QueryHandlerDebug = 1011,
    QueryHandlerInformation = 1012,
    QueryHandlerWarning = 1013,
    QueryHandlerError = 1014,

    CommandHandlerTrace = 1020,
    CommandHandlerDebug = 1021,
    CommandHandlerInformation = 1022,
    CommandHandlerWarning = 1023,
    CommandHandlerError = 1024,

    RuleCheckerTrace = 1030,
    RuleCheckerDebug = 1031,
    RuleCheckerInformation = 1032,
    RuleCheckerWarning = 1033,
    RuleCheckerError = 1034,

    WorkerStart = 1040,
    WorkerFinished = 1041,
    WorkServiceError = 1042,
}

public static class EventIdFactory
{
    public static EventId Create(ReservedLogEventId reservedLogEventId)
    {
        return new EventId((int)reservedLogEventId, reservedLogEventId.ToString());
    }
}
