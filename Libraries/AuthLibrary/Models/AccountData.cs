using AuthDb;
using CommonLibrary.Models;
using Mapster;

namespace AuthLibrary.Models;

public sealed record AccountData(string Token, string AccountId, DateTime CreatedAt, AccountRoles Role);

internal sealed class AccountDataRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Account, AccountData>();
    }
}