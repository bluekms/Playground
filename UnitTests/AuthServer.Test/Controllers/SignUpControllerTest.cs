using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Account;
using AuthServer.Models;
using AuthServer.Test.Models;
using CommonLibrary;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace AuthServer.Test.Controllers
{
    public class SignUpControllerTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ITimeService timeService;

        public SignUpControllerTest()
        {
            authDbFixture = new();
            dbContext = authDbFixture.CreateContext();

            mapper = InitMapper.Use();
            timeService = new ScopedTimeService();
        }

        public void Dispose()
        {
            dbContext.Dispose();
            authDbFixture.Dispose();
        }

        [Theory]
        [InlineData("bluekms1", "1234", UserRoles.User)]
        public async void SignUp(string accountId, string password, UserRoles role)
        {
            var controller = new SignUpController(
                new SignUpRuleChecker(new GetAccountHandler(dbContext, mapper)),
                new AddAccountHandler(dbContext, mapper, timeService));

            var result = await controller.SignUp(new(accountId, password, role));
            var actionResult = Assert.IsType<ActionResult<AccountData>>(result);

            actionResult.Value?.CreatedAt.ShouldBe(timeService.Now);
        }
    }
}