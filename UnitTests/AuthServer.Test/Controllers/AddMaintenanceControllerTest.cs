using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Maintenance;
using AuthServer.Test.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace AuthServer.Test.Controllers
{
    public sealed class AddMaintenanceControllerTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ConnectionMultiplexer redisConnection;

        public AddMaintenanceControllerTest()
        {
            authDbFixture = new();
            dbContext = authDbFixture.CreateContext();

            var config = InitConfig.Use();
            redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache"));

            mapper = InitMapper.Use();
        }

        public void Dispose()
        {
            authDbFixture.Dispose();
            dbContext.Dispose();
            redisConnection.Dispose();
        }

        [Theory]
        [InlineData("2022-03-09 13:00", "2022-03-09 14:00", "Unit Testing")]
        public async void AddMaintenance(DateTime start, DateTime end, string reason)
        {
            var controller = new AddMaintenanceController(
                mapper,
                new CheckMaintenanceRule(dbContext),
                new AddMaintenanceHandler(dbContext, mapper));

            var result = await controller.AddMaintenance(new(start, end, reason));
            var actionResult = Assert.IsType<ActionResult<AddMaintenanceController.Returns>>(result);
            actionResult.Value?.Id.ShouldBe(1);

            result = await controller.AddMaintenance(new(start.AddDays(1), end.AddDays(1), reason));
            actionResult = Assert.IsType<ActionResult<AddMaintenanceController.Returns>>(result);
            actionResult.Value?.Id.ShouldBe(2);

            result = await controller.AddMaintenance(new(start.AddDays(3), end.AddDays(3), reason));
            actionResult = Assert.IsType<ActionResult<AddMaintenanceController.Returns>>(result);
            actionResult.Value?.Id.ShouldBe(3);
        }
    }
}