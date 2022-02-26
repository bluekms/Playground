using System;
using AuthDb;
using CommonLibrary.Models;
using Mapster;

namespace AccountServer.Models
{
    public sealed record AccountData(string Token, string AccountId, DateTime CreatedAt, UserRoles Role);

    internal sealed class AccountDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Account, AccountData>();
        }
    }
}