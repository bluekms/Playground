using System;
using AuthDb;
using Mapster;

namespace AuthServer.Models
{
    public sealed record MaintenanceData(long Id, DateTime Start, DateTime End, string Reason);

    internal sealed class MaintenanceDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Maintenance, MaintenanceData>();
        }
    }
}
