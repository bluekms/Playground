using System.Threading;
using AuthServer.Controllers;
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
        var controller1 = new FooController();
        var result1 = controller1.Foo(new(foo), CancellationToken.None);
        var actionResult1 = Assert.IsType<ActionResult<string>>(result1);
        
        var controller2 = new FooWithoutAuthController();
        var result2 = controller2.Foo(new(foo), CancellationToken.None);
        var actionResult2 = Assert.IsType<ActionResult<string>>(result2);
        
        Assert.Equal($"{foo}: Ok", actionResult1.Value);
        Assert.Equal($"{foo}: Ok", actionResult2.Value);
    }
}