using System;
using Mapster;

namespace AccountServer.Models
{
    public sealed record AccountData(string AccountId, string SessionId, DateTime CreatedAt, string Authority);

    internal sealed class AccountDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AuthContext.Account, AccountData>();
        }
    }
}