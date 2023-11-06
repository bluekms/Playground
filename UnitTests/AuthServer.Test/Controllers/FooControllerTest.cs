using System.Threading;
using AuthLibrary.Feature.Session;
using AuthServer.Controllers;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AuthServer.Test.Controllers;

public sealed class FooControllerTest
{
    [Theory]
    [InlineData("Kms")]
    [InlineData("UnitTest")]
    public void Foo(string foo)
    {
        var controller1 = new FooWithoutAuthController();
        var result1 = controller1.Foo(new(new ReqFoo { Data = foo, }), CancellationToken.None);
        var actionResult1 = Assert.IsType<ActionResult<string>>(result1);

        var session = new SessionInfo("TEST", AccountRoles.User);
        
        var controller2 = new FooController();
        var result2 = controller2.Foo(session, new(new ReqFoo { Data = foo }), CancellationToken.None);
        var actionResult2 = Assert.IsType<ActionResult<string>>(result2);
        
        Assert.Equal($"{foo}: Ok.", actionResult1.Value);
        Assert.StartsWith($"{foo}: Ok.", actionResult2.Value);
    }
}