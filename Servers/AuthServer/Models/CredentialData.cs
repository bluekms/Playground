using AuthDb;
using CommonLibrary.Models;
using Mapster;

namespace AccountServer.Models
{
    public sealed record CredentialData(string Token, string Name, ServerRoles Role, string Description);

    internal sealed class CredentialDataRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Credential, CredentialData>();
        }
    }
}