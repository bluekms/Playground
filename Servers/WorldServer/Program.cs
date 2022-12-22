using System.Reflection;
using AuthDb;
using AuthLibrary.Extensions;
using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using CommonLibrary;
using CommonLibrary.Extensions;
using CommonLibrary.Handlers;
using CommonLibrary.Handlers.Decorators;
using CommonLibrary.Models;
using StaticDataLibrary.Extensions;
using WorldServer;
using WorldServer.Extensions;
using AssemblyEntry = AuthLibrary.Models.AssemblyEntry;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Services.UseNginx();
builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString("AuthDb"));
builder.Services.UseMySql<WorldDbContext>(builder.Configuration.GetConnectionString("WorldDb"));
builder.Services.UseStaticData("StaticData");
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache")!);
builder.Services.UseMapster();
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UsePermissionAuthorization();
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
app.UseForwardedHeaders();
app.UseAuthorization();
app.MapControllers();
app.Run();