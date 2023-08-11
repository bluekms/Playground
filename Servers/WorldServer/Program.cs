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
using Serilog;
using StaticDataLibrary.Extensions;
using StaticDataLibrary.Options;
using WorldDb;
using WorldServer.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Host.UseStashbox();
builder.Services.UseNginx();
builder.Services.UseMapster();
builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString(AuthDbContext.ConfigurationSection));
builder.Services.UseMySql<WorldDbContext>(builder.Configuration.GetConnectionString(WorldDbContext.ConfigurationSection));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString(RedisCacheExtension.ConfigurationSection)!);
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UseOpenAuthentication();
builder.Services.UsePermissionAuthorization();
builder.Services.UseHandlers(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(AuthLibrary.Models.AssemblyEntry))!);
builder.Services.UseServerRegistry(builder.Configuration.GetSection(ServerRegistryOptions.ConfigurationSection));
builder.Services.UseControllers();

// DI
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

await builder.Services.UseStaticDataAsync(builder.Configuration.GetSection(StaticDataOptions.ConfigurationSection));

// Configure the HTTP request pipeline.
//
var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseForwardedHeaders();
app.UseAuthorization();
app.MapControllers();
app.Run();
