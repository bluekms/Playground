using Microsoft.AspNetCore.Authorization;

namespace AccountServer.Extensions.Authorizations
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