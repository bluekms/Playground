using System;
using AuthDb;
using AuthServer.Controllers;
using AuthServer.Handlers.Server;
using AuthServer.Test.Models;
using CommonLibrary.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Xunit;

namespace AuthServer.Test.Controllers
{
    public class AddServerControllerTest : IDisposable
    {
        private readonly AuthDbFixture authDbFixture;
        private readonly AuthContext context;
        private readonly IMapper mapper;
        private readonly ConnectionMultiplexer redisConnection;

        public AddServerControllerTest()
        {
            authDbFixture = new();
            context = authDbFixture.CreateContext();
            
            var config = InitConfig.Use();
            redisConnection = ConnectionMultiplexer.Connect(config.GetConnectionString("RedisCache"));
            
            mapper = InitMapper.Use();
        }

        public void Dispose()
        {
            authDbFixture.Dispose();
            context.Dispose();
            redisConnection.Dispose();
        }

        [Theory]
        [InlineData("KmsWorld", ServerRoles.World, "localhost:1234", "2022-03-10", "xUnit Test")]
        public async void AddServer(string name, ServerRoles role, string address, DateTime expireAt, string description)
        {
            var controller = new AddServerController(
                mapper,
                new UpsertServerHandler(context, mapper));

            var result = await controller.AddServer(new(name, role, address, expireAt, description));
            
            Assert.IsType<OkResult>(result);
        }
    }
}