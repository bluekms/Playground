using CommonLibrary.Models;

namespace AuthLibrary.Feature.Session;

public sealed class SessionData
{
    public SessionData(string sessionId, AccountRoles roles)
    {
        SessionId = sessionId;
        Roles = roles;
    }

    public string SessionId { get; }

    public AccountRoles Roles { get; }

    public DeviceInfo? Device { get; init; }

    public UserInfo? User { get; init; }
}

public sealed record DeviceInfo(string ClientVersion, string StaticDataVersion);

public sealed record UserInfo(long Usn);