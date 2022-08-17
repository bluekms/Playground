using System.Reflection;
using AuthDb;
using AuthLibrary.Extensions;
using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Models;
using CommonLibrary;
using CommonLibrary.Extensions;
using CommonLibrary.Handlers;
using CommonLibrary.Handlers.Decorators;
using CommonLibrary.Models;
using WorldServer;
using WorldServer.Extensions;
using WorldServer.Services;

// https://docs.microsoft.com/ko-kr/aspnet/core/grpc/?view=aspnetcore-6.0
// how to use both grpc and controller in asp dotnet core
//
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();

builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString("AuthDb"));
builder.Services.UseMySql<WorldDbContext>(builder.Configuration.GetConnectionString("WorldDb"));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache"));

builder.Services.UseMapster();
builder.Services.UseSessionAuthentication();
builder.Services.UsePermissionAuthorization();

builder.Services.UseHandlers(new GenericDerivedTypeSelector(Assembly.GetExecutingAssembly()));
builder.Services.UseHandlers(new GenericDerivedTypeSelector(Assembly.GetAssembly(typeof(AssemblyEntry))!));
builder.Services.UseQueryDecorator();
builder.Services.UseCommandDecorator();

builder.Services.UseServerRegistry(builder.Configuration.GetSection(ServerRegistryOptions.ConfigurationSection));
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

builder.Services.UseControllers();
builder.Services.UseGrpc();

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    // Configure the HTTP request pipeline.
    endpoints.MapGrpcService<FooService>();
    endpoints.MapGrpcService<GreeterService>();
});

app.MapControllers();
app.Run();