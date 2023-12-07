namespace CommonLibrary.Models;

public sealed class ServerRegistryOptions
{
    public const string ConfigurationSection = "ServerRegistry";

    public string AuthServerAddress { get; init; } = null!;

    public string Token { get; init; } = null!;

    public string Name { get; init; } = null!;

    public ServerRoles Role { get; init; }

    public string Address { get; init; } = null!;

    public string Description { get; init; } = null!;

    public long ExpireSec { get; init; }
}
