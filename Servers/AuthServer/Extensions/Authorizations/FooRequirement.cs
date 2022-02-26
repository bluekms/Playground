using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Extensions.Authorizations
{
    public class FooRequirement : IAuthorizationRequirement
    {
        public FooRequirement(string test)
        {
            Test = test;
        }

        public string Test { get; }
    }
}