using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Session;
using AuthServer.Handlers.World;
using AuthServer.Test.Models;
using CommonLibrary;
using CommonLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using StackExchange.Redis;
using Xunit;
using IMapper = MapsterMapper.IMapper;

namespace AuthServer.Test.Controllers
{
    public class LoginControllerTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthContext context;
        private readonly IMapper mapper;
        private readonly ConnectionMultiplexer redisConnection;
        private readonly ITimeService timeService;

        public LoginControllerTest()
        {
            authDbFixture = new();
            context = authDbFixture.CreateContext();
            
            var config = InitConfig.Use();
            redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache"));
            
            mapper = InitMapper.Use();
            timeService = new ScopedTimeService();
            
            InitData();
        }
        
        public void Dispose()
        {
            authDbFixture.Dispose();
            context.Dispose();
            redisConnection.Dispose();
        }
        
        [Theory]
        [InlineData("bluekms", "1234")]
        public async void Login(string accountId, string password)
        {
            var mockLogger = new Mock<ILogger<SignUpController>>();

            var rule = new LoginRuleChecker(context);
            var deleteSession = new DeleteSessionHandler(redisConnection.GetDatabase());
            var updateSession = new UpdateSessionHandler(context, mapper);
            var insertSession = new InsertSessionHandler(redisConnection.GetDatabase());
            var getServerList = new GetServerListHandler(context, timeService, mapper);
            
            var controller = new LoginController(
                mockLogger.Object,
                rule,
                deleteSession,
                updateSession,
                insertSession,
                getServerList);
            
            var result = await controller.Login(new(accountId, password));
            var actionResult = Assert.IsType<ActionResult<LoginController.Returns>>(result);

            actionResult.Value?.SessionToken.ShouldNotBeNull();
            actionResult.Value?.Worlds.Count.ShouldBe(2);
        }
        
        
        private void InitData()
        {
            context.Accounts.Add(new()
            {
                Token = string.Empty,
                AccountId = "bluekms",
                Password = "1234",
                CreatedAt = DateTime.Now,
                Role = UserRoles.Administrator,
            });

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