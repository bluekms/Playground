using System;
using AuthDb;
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

namespace AuthServer.Test.Scenarios;

public sealed class AccountScenarioTest : IDisposable
{
    private readonly AuthDbFixture authDbFixture;
    private readonly AuthDbContext dbContext;
    private readonly ConnectionMultiplexer redisConnection;
    private readonly IMapper mapper;
    private readonly ITimeService timeService;

    public AccountScenarioTest()
    {
        authDbFixture = new();
        dbContext = authDbFixture.CreateContext();

        var config = InitConfig.Use();
        redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache"));

        mapper = InitMapper.Use();
        timeService = new ScopedTimeService();
    }

    public void Dispose()
    {
        authDbFixture.Dispose();
        dbContext.Dispose();
        redisConnection.Dispose();
    }

    [Fact]
    public async void AccountScenario()
    {
        InitData();

        var accountId = "bluekms";
        var password = "1234";

        var signUpController = new SignUpController(
            new SignUpRuleChecker(new GetAccountHandler(dbContext, mapper)),
            new AddAccountHandler(dbContext, mapper, timeService),
            mapper);

        var resultSignUp = await signUpController.SignUp(new(accountId, password));
        resultSignUp.Value.ShouldNotBeNull();
        resultSignUp.Value?.AccountId.ShouldBe(accountId);
        resultSignUp.Value?.Role.ShouldBe(UserRoles.User);

        var loginController = new LoginController(
            new LoginRuleChecker(dbContext),
            new DeleteSessionHandler(redisConnection.GetDatabase()),
            new UpdateSessionHandler(dbContext, mapper),
            new AddSessionHandler(redisConnection.GetDatabase()),
            new GetServerListHandler(dbContext, timeService, mapper));

        var resultLogin = await loginController.Login(new(accountId, password));
        var actionResultLogin = Assert.IsType<ActionResult<LoginController.Result>>(resultLogin);

        actionResultLogin.Value?.SessionId.ShouldNotBeNull();
        actionResultLogin.Value?.Worlds.Count.ShouldBe(2);
    }

    private void InitData()
    {
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