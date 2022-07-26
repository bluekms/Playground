using System;

namespace CommonLibrary.Models
{
    public sealed class ServerRegistryOptions
    {
        public const string ConfigurationSection = "ServerRegistry";

        public string AuthServerAddress { get; set; } = default!;

        public string Token { get; set; } = default!;

        public string Name { get; set; } = default!;

        public ServerRoles Role { get; set; }

        public string Address { get; set; } = default!;

        public string Description { get; set; } = default!;

        public long ExpireSec { get; set; }
    }
}