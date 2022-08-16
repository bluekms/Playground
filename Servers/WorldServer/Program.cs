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
//
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();

builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString("AuthDb"));
builder.Services.UseMySql<WorldDbContext>(builder.Configuration.GetConnectionString("WorldDb"));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache"));
builder.Services.UseMapster();
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UsePermissionAuthorization();

builder.Services.UseHandlers(new GenericDerivedTypeSelector(Assembly.GetExecutingAssembly()));
builder.Services.UseHandlers(new GenericDerivedTypeSelector(Assembly.GetAssembly(typeof(AssemblyEntry))!));
builder.Services.UseQueryDecorator();
builder.Services.UseCommandDecorator();

builder.Services.UseServerRegistry(builder.Configuration.GetSection(ServerRegistryOptions.ConfigurationSection));
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

builder.Services.UseControllers();

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
//

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
