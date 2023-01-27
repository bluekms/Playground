using System;
using System.Threading;
using AuthDb;
using AuthLibrary.Feature.Session;
using AuthLibrary.Handlers;
using AuthServer.Controllers;
using AuthServer.Handlers.Account;
using AuthServer.Handlers.Session;
using AuthServer.Handlers.World;
using AuthServer.Test.Models;
using CommonLibrary;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace AuthServer.Test.Controllers;

public sealed class LoginControllerTest : IDisposable
{
    private readonly AuthDbFixture authDbFixture;
    private readonly AuthDbContext dbContext;
    private readonly ConnectionMultiplexer redisConnection;
    private readonly IMapper mapper;
    private readonly ITimeService timeService;

    public LoginControllerTest()
    {
        authDbFixture = new();
        dbContext = authDbFixture.CreateContext();

        var config = InitConfig.Use();
        redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache")!);

        mapper = InitMapper.Use();
        timeService = new ScopedTimeService();

        InitData();
    }

    public void Dispose()
    {
        authDbFixture.Dispose();
        dbContext.Dispose();
        redisConnection.Dispose();
    }

    [Theory]
    [InlineData("bluekms", "1234")]
    public async void Login(string accountId, string password)
    {
        var controller = new LoginController(
            new LoginRuleChecker(dbContext),
            new UpdateSessionHandler(redisConnection, dbContext, mapper),
            new SessionStore(redisConnection),
            new GetServerListHandler(dbContext, timeService, mapper));

        var result = await controller.Login(new(accountId, password), CancellationToken.None);
        var actionResult = Assert.IsType<ActionResult<LoginController.Result>>(result);

        actionResult.Value?.SessionId.ShouldNotBeNull();
        actionResult.Value?.Worlds.Count.ShouldBe(2);
    }

    private void InitData()
    {
        dbContext.Accounts.Add(new()
        {
            Token = string.Empty,
            AccountId = "bluekms",
            Password = "1234",
            CreatedAt = DateTime.Now,
            Role = AccountRoles.Administrator,
        });

        dbContext.Servers.Add(new()
        {
            Name = "a",
            Role = ServerRoles.Auth,
            Address = string.Empty,
            ExpireAt = DateTime.Now.AddDays(1),
            Description = "Unit Test Auth Server"
        });

        dbContext.Servers.Add(new()
        {
            Name = "b",
            Role = ServerRoles.Operation,
            Address = string.Empty,
            ExpireAt = DateTime.Now.AddDays(1),
            Description = "Unit Test Op Server"
        });

        dbContext.Servers.Add(new()
        {
            Name = "c",
            Role = ServerRoles.World,
            Address = string.Empty,
            ExpireAt = DateTime.Now.AddDays(1),
            Description = "Unit Test World Server 1"
        });

        dbContext.Servers.Add(new()
        {
            Name = "d",
            Role = ServerRoles.World,
            Address = string.Empty,
            ExpireAt = DateTime.Now.AddDays(1),
            Description = "Unit Test World Server 2"
        });

        dbContext.SaveChanges();
    }
}