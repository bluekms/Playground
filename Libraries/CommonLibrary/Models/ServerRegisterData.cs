using System;

namespace CommonLibrary.Models
{
    public sealed class ServerRegisterData
    {
        public string Token { get; init; } = default!;
        public string ServerName { get; init; } = default!;
        public string ServerRole { get; init; } = default!;
        public string Address { get; init; } = default!;
        public DateTime ExpireAt { get; init; } = default!;
        public string Description { get; init; } = default!;
    }
}