using System;
using AuthDb;
using Mapster;

namespace AccountServer.Models
{
    public sealed record AccountData(string AccountId, string SessionId, DateTime CreatedAt, string UserRole);

    internal sealed class AccountDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Account, AccountData>();
        }
    }
}