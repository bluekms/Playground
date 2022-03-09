using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Server;
using AuthServer.Models;
using AuthServer.Test.Models;
using CommonLibrary;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace AuthServer.Test.Scenarios
{
    public class AccountScenarioTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthContext context;
        private readonly IMapper mapper;
        private readonly ITimeService timeService;

        public AccountScenarioTest()
        {
            authDbFixture = new();
            context = authDbFixture.CreateContext();
            
            mapper = InitMapper.Use();
            timeService = new ScopedTimeService();
        }
        
        [Theory]
        [InlineData("KmsWorld", ServerRoles.World, "localhost:1234", "2022-03-10", "xUnit Test")]
        [InlineData("KmsWorld2", ServerRoles.World, "localhost:1234", "2022-03-10", "xUnit Test")]
        public async void AddServer(string name, ServerRoles role, string address, DateTime expireAt, string description)
        {
            var controller = new AddServerController(
                mapper,
                new UpsertServerHandler(context, mapper));

            var result = await controller.AddServer(new(name, role, address, expireAt, description));
            
            Assert.IsType<OkResult>(result);
        }

        public void Dispose()
        {
            authDbFixture.Dispose();
            context.Dispose();
        }
        
        [Theory]
        [InlineData("bluekms1", "1234", UserRoles.User)]
        public async void SignUp(string accountId, string password, UserRoles role)
        {
            var controller = new SignUpController(
                new SignUpRuleChecker(new GetAccountHandler(context, mapper)),
                new AddAccountHandler(context, mapper, timeService));

            var result = await controller.SignUp(new(accountId, password, role));
            var actionResult = Assert.IsType<ActionResult<AccountData>>(result);
            
            actionResult.Value?.CreatedAt.ShouldBe(timeService.Now);
        }
    }
}