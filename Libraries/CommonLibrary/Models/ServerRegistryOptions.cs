using System;

namespace CommonLibrary.Models
{
    public sealed class ServerRegistryOptions
    {
        public string Address { get; init; } = default!;
        public string Token { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string Role { get; init; } = default!;
        public DateTime ExpireAt { get; init; } = default!;
    }
}