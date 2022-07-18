using Mapster;

namespace AuthServer.Test
{
    public class InitMapper
    {
        public static MapsterMapper.Mapper Use()
        {
            var config = new TypeAdapterConfig();
            config.RequireDestinationMemberSource = true;
            config.Default.MapToConstructor(true);

            return new MapsterMapper.Mapper(config);
        }
    }
}