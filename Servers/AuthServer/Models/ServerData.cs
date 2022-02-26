using AuthDb;
using Mapster;

namespace AccountServer.Models
{
    public sealed record ServerData(string Name, string Address);

    internal sealed class ServerDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Server, ServerData>();
        }
    }
}