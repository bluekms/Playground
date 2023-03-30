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
using StaticDataLibrary.Options;
using WorldServer;
using WorldServer.Extensions;
using AssemblyEntry = AuthLibrary.Models.AssemblyEntry;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Services.UseNginx();
builder.Services.UseMapster();
builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString(AuthDbContext.SectionName));
builder.Services.UseMySql<WorldDbContext>(builder.Configuration.GetConnectionString(WorldDbContext.SectionName));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString(RedisCacheExtension.SectionName)!);
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UseOpenAuthentication();
builder.Services.UsePermissionAuthorization();
builder.Services.UseHandlers(new(Assembly.GetExecutingAssembly()));
builder.Services.UseHandlers(new(Assembly.GetAssembly(typeof(AssemblyEntry))!));
builder.Services.UseQueryDecorator();
builder.Services.UseCommandDecorator();
builder.Services.UseServerRegistry(builder.Configuration.GetSection(ServerRegistryOptions.ConfigurationSection));
builder.Services.UseControllers();

await builder.Services.UseStaticDataAsync(builder.Configuration.GetSection(StaticDataOptions.SectionName));

// DI
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.
//
var app = builder.Build();
app.UseForwardedHeaders();
app.UseAuthorization();
app.MapControllers();
app.Run();