using Microsoft.AspNetCore.Authorization;

namespace AuthLibrary.Extensions.Authorizations;

public class BuildConfigurationRequirement : IAuthorizationRequirement
{
    public const string ClaimType = "BuildConfiguration";

    public BuildConfigurationRequirement(BuildConfigurations buildConfiguration)
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