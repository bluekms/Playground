using System.Text.Json;
using StackExchange.Redis;

namespace AuthLibrary.Feature.Session;

public sealed class SessionStore
{
    private const string SessionPrefix = "Session";

    private readonly IDatabase redis;

    public SessionStore(IConnectionMultiplexer multiplexer)
    {
        redis = multiplexer.GetDatabase();
    }

    public async Task SetAsync(SessionInfo session, CancellationToken cancellationToken)
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(session);
        await redis.StringSetAsync($"{SessionPrefix}:{session.SessionId}", data);
    }

    public async Task<SessionInfo> GetAsync(string sessionId, CancellationToken cancellationToken)
    {
        var data = await redis.StringGetAsync($"{SessionPrefix}:{sessionId}");
        if (data.HasValue is false)
        {
            throw new InvalidDataException(nameof(sessionId));
        }

        var session = JsonSerializer.Deserialize<SessionInfo>(data!);
        if (session is null)
        {
            throw new InvalidCastException($"{nameof(sessionId)}: {sessionId}, {data}");
        }

        return session;
    }

    public async Task DeleteAsync(string sessionId, CancellationToken cancellationToken)
    {
        await redis.KeyDeleteAsync($"{SessionPrefix}:{sessionId}");
    }
}
