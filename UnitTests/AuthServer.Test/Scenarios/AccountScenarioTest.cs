using System;
using System.Collections.Generic;
using System.Threading;
using AuthDb;
using AuthLibrary.Feature.Session;
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
    private readonly ConnectionMultiplexer redisMultiplexer;
    private readonly IMapper mapper;
    private readonly ITimeService timeService;
    private readonly IConfigurationRoot appConfig;

    public AccountScenarioTest()
    {
        authDbFixture = new();
        dbContext = authDbFixture.CreateContext();

        var config = InitConfig.Use();
        redisMultiplexer = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache")!);

        mapper = InitMapper.Use();
        timeService = new ScopedTimeService();

        appConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("AppSecrets:AccountSalt", "Foo"),
                new KeyValuePair<string, string>("AppSecrets:SessionPrefix", "Bar")
            }!).Build();
    }

    public void Dispose()
    {
        authDbFixture.Dispose();
        dbContext.Dispose();
        redisMultiplexer.Dispose();
    }

    [Fact]
    public async void AccountScenario()
    {
        InitData();

        var accountId = "bluekms";
        var password = "1234";

        var signUpController = new SignUpController(
            appConfig,
            new SignUpRuleChecker(dbContext),
            new SignUpHandler(timeService, dbContext));

        var resultSignUp = await signUpController.SignUp(new(accountId, password), CancellationToken.None);
        resultSignUp.ShouldNotBeNull();
        resultSignUp.AccountId.ShouldBe(accountId);
        resultSignUp.Role.ShouldBe(ResSignUp.Types.AccountRoles.User);

        var loginController = new LoginController(
            appConfig,
            new LoginRuleChecker(dbContext),
            new UpdateSessionHandler(redisMultiplexer, dbContext, mapper),
            new SessionStore(redisMultiplexer),
            new GetServerListHandler(dbContext, timeService, mapper));

        var resultLogin = await loginController.Login(new(accountId, password), CancellationToken.None);
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
