using System.Reflection;
using AuthDb;
using AuthLibrary.Extensions;
using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Models;
using CommonLibrary;
using CommonLibrary.Extensions;
using CommonLibrary.Handlers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Host.UseStashbox();
builder.Services.UseNginx();
builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString(AuthDbContext.ConfigurationSection));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString(RedisCacheExtension.ConfigurationSection)!);
builder.Services.UseMapster();
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UseOpenAuthentication();
builder.Services.UsePermissionAuthorization();
builder.Services.UseHandlers(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(AssemblyEntry))!);
builder.Services.UseControllers();

// 추가 DI
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseForwardedHeaders();
app.UseAuthorization();
app.MapControllers();
app.Run();