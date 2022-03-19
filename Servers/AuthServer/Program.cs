using System.Reflection;
using AuthServer.Extensions;
using AuthServer.Extensions.Authentication;
using AuthServer.Extensions.Authorizations;
using CommonLibrary;
using CommonLibrary.Handlers;
using CommonLibrary.Handlers.Decorators;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Host.UseStashbox();

builder.Services.UseControllers();
builder.Services.UseMySql(builder.Configuration.GetConnectionString("AuthDb"));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache"));
builder.Services.UseMapster(Assembly.GetExecutingAssembly());
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UsePermissionAuthorization();

var typeSelector = new GenericDerivedTypeSelector(Assembly.GetExecutingAssembly());
builder.Services.UseQueryDecorator();
builder.Services.UseCommandDecorator();
builder.Services.UseHandlers(typeSelector);

builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.

var app = builder.Build();
app.UseAuthorization();
app.MapControllers();
app.Run();
