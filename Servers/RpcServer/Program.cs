using System.Reflection;
using AuthLibrary.Extensions;
using CommonLibrary;
using CommonLibrary.Extensions;
using CommonLibrary.Handlers;
using CommonLibrary.Handlers.Decorators;
using CommonLibrary.Models;
using RpcServer.Services;
using StaticDataLibrary.Extensions;
using AssemblyEntry = AuthLibrary.Models.AssemblyEntry;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Host.UseLogger();
builder.Services.UseNginx();
builder.Services.UseStaticData("StaticData");
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache")!);
builder.Services.UseMapster();
builder.Services.UseHandlers(new(Assembly.GetExecutingAssembly()));
builder.Services.UseHandlers(new(Assembly.GetAssembly(typeof(AssemblyEntry))!));
builder.Services.UseQueryDecorator();
builder.Services.UseCommandDecorator();
builder.Services.UseServerRegistry(builder.Configuration.GetSection(ServerRegistryOptions.ConfigurationSection));
builder.Services.UseControllers();

// DI
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.
//
var app = builder.Build();
app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();