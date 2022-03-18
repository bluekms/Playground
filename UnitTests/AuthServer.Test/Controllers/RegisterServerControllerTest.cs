using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Server;
using AuthServer.Test.Models;
using CommonLibrary;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Xunit;

namespace AuthServer.Test.Controllers
{
    public class RegisterServerControllerTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthContext context;
        private readonly ITimeService timeService;
        private readonly IMapper mapper;
        private readonly ConnectionMultiplexer redisConnection;

        public RegisterServerControllerTest()
        {
            authDbFixture = new();
            context = authDbFixture.CreateContext();
            
            var config = InitConfig.Use();
            redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache"));

            timeService = new ScopedTimeService();
            mapper = InitMapper.Use();
        }

        public void Dispose()
        {
            authDbFixture.Dispose();
            context.Dispose();
            redisConnection.Dispose();
        }

        [Theory]
        [InlineData("KmsWorld", ServerRoles.World, "localhost:1234", "xUnit Test", 60 * 5)]
        public async void AddServer(string name, ServerRoles role, string address, string description, long expireSec)
        {
            var controller = new RegisterServerController(
                mapper,
                new UpsertServerHandler(context, timeService, mapper));

            var result = await controller.RegisterServer(new(name, role, address, description, expireSec));
            
            Assert.IsType<OkResult>(result);
        }
    }
}