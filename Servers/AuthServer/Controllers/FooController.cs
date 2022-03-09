using System;
using System.Threading.Tasks;
using AuthServer.Extensions.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthServer.Controllers
{
    [ApiController]
    public class FooController : ControllerBase
    {
        private readonly ILogger<FooController> logger;

        public FooController(ILogger<FooController> logger)
        {
            this.logger = logger;
        }

        [HttpPost, Route("Auth/Foo")]
        [Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
        public async Task<ActionResult<string>> Foo([FromBody] ArgumentData args)
        {
            try
            {
                return await HandleAsync(args);
            }
            catch (Exception e)
            {
                logger.LogError($"{e.Message}:{e.InnerException?.Message ?? string.Empty}");
                return NotFound();
            }
        }

        private async Task<string> HandleAsync(ArgumentData args)
        {
            return await Task.Run(() => args.Data + ": OK");
        }

        public sealed record ArgumentData(string Data);
    }
}