using System;
using AuthDb;
using Mapster;

namespace AccountServer.Models
{
    public sealed record WorldData(string WorldName, string Address);

    internal sealed class WorldDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<World, WorldData>();
        }
    }
}