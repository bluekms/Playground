using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Server;
using AuthServer.Handlers.Session;
using AuthServer.Handlers.World;
using AuthServer.Models;
using AuthServer.Test.Models;
using CommonLibrary;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace AuthServer.Test.Scenarios
{
    public class AccountScenarioTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthContext context;
        private readonly ConnectionMultiplexer redisConnection;
        private readonly IMapper mapper;
        private readonly ITimeService timeService;

        public AccountScenarioTest()
        {
            authDbFixture = new();
            context = authDbFixture.CreateContext();
            
            var config = InitConfig.Use();
            redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache"));
            
            mapper = InitMapper.Use();
            timeService = new ScopedTimeService();
        }
        
        public void Dispose()
        {
            authDbFixture.Dispose();
            context.Dispose();
            redisConnection.Dispose();
        }
        
        [Fact]
        public async void AccountScenario()
        {
            InitData();

            var accountId = "bluekms";
            var password = "1234";
            var role = UserRoles.User;
            
            var signUpController = new SignUpController(
                new SignUpRuleChecker(new GetAccountHandler(context, mapper)),
                new AddAccountHandler(context, mapper, timeService));

            var resultSignUp = await signUpController.SignUp(new(accountId, password, role));
            var actionResultSignUp = Assert.IsType<ActionResult<AccountData>>(resultSignUp);
            
            actionResultSignUp.Value?.CreatedAt.ShouldBe(timeService.Now);
            
            var loginController = new LoginController(
                new LoginRuleChecker(context),
                new DeleteSessionHandler(redisConnection.GetDatabase()),
                new UpdateSessionHandler(context, mapper),
                new AddSessionHandler(redisConnection.GetDatabase()),
                new GetServerListHandler(context, timeService, mapper));
            
            var resultLogin = await loginController.Login(new(accountId, password));
            var actionResultLogin = Assert.IsType<ActionResult<LoginController.Returns>>(resultLogin);

            actionResultLogin.Value?.SessionToken.ShouldNotBeNull();
            actionResultLogin.Value?.Worlds.Count.ShouldBe(2);
        }
        
        private void InitData()
        {
            context.Servers.Add(new()
            {
                Name = "a",
                Role = ServerRoles.Auth,
                Address = string.Empty,
                ExpireAt = DateTime.Now.AddDays(1),
                Description = "Unit Test Auth Server"
            });
            
            context.Servers.Add(new()
            {
                Name = "b",
                Role = ServerRoles.Operation,
                Address = string.Empty,
                ExpireAt = DateTime.Now.AddDays(1),
                Description = "Unit Test Op Server"
            });
            
            context.Servers.Add(new()
            {
                Name = "c",
                Role = ServerRoles.World,
                Address = string.Empty,
                ExpireAt = DateTime.Now.AddDays(1),
                Description = "Unit Test World Server 1"
            });
            
            context.Servers.Add(new()
            {
                Name = "d",
                Role = ServerRoles.World,
                Address = string.Empty,
                ExpireAt = DateTime.Now.AddDays(1),
                Description = "Unit Test World Server 2"
            });
            
            context.SaveChanges();
        }
    }
}