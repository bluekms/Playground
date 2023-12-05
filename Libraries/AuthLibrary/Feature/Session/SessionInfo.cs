using CommonLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthLibrary.Feature.Session;

[ModelBinder(BinderType = typeof(SessionBinder))]
public sealed class SessionInfo
{
    public SessionInfo(string sessionId, ResSignUp.Types.AccountRoles accountRole)
    {
        SessionId = sessionId;
        AccountRole = accountRole;
    }

    public string SessionId { get; }

    public ResSignUp.Types.AccountRoles AccountRole { get; }

    public DeviceInfo? Device { get; init; }

    public UserInfo? User { get; init; }
}

public sealed record DeviceInfo(string ClientVersion, string StaticDataVersion);

public sealed record UserInfo(long Usn);