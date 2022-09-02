using System.Net;
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
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Services.UseNginx();
builder.Services.UseMySql<AuthDbContext>(builder.Configuration.GetConnectionString("AuthDb"));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache"));
builder.Services.UseMapster();
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UsePermissionAuthorization();
builder.Services.UseHandlers(new GenericDerivedTypeSelector(Assembly.GetExecutingAssembly()));
builder.Services.UseHandlers(new GenericDerivedTypeSelector(Assembly.GetAssembly(typeof(AssemblyEntry))!));
builder.Services.UseQueryDecorator();
builder.Services.UseCommandDecorator();
builder.Services.UseControllers();

// DI
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseForwardedHeaders();
app.UseAuthorization();
app.MapControllers();
app.Run();