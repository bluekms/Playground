using System;

namespace AccountServer.Models
{
    public sealed record AccountData(string AccountId, string SessionId, DateTime CreatedAt, string Authority);
}