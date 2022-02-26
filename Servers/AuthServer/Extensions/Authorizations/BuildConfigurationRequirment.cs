using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Extensions.Authorizations
{
    public class BuildConfigurationRequirment : IAuthorizationRequirement
    {
        public const string ClaimType = "BuildConfiguration";

        public BuildConfigurationRequirment(BuildConfigurations buildConfiguration)
        {
            BuildConfiguration = buildConfiguration;
        }

        public enum BuildConfigurations
        {
            Debug,
            Release,
        }

        public BuildConfigurations BuildConfiguration { get; }
    }
}