using AuthServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AuthServer.Test.Controllers
{
    public class FooControllerTest
    {
        [Theory]
        [InlineData("Kms")]
        [InlineData("UnitTest")]
        public async void Foo(string foo)
        {
            var mockLogger = new Mock<ILogger<FooController>>();
            var controller = new FooController(mockLogger.Object);

            var result = await controller.Foo(new(foo));

            var actionResult = Assert.IsType<ActionResult<string>>(result);
            Assert.Equal($"{foo}: OK", actionResult.Value);
        }
    }
}