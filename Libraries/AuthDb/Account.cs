using System;
using System.Diagnostics.CodeAnalysis;

namespace AuthDb
{
    public sealed class Account
    {
        [AllowNull]
        public string AccountId { get; set; }

        [AllowNull]
        public string Password { get; set; }

        [AllowNull]
        public string SessionId { get; set; }

        public DateTime CreatedAt { get; set; }

        [AllowNull]
        public string Authority { get; set; }
    }
}