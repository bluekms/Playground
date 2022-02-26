using System.Reflection;
using AccountServer.Extensions;
using AccountServer.Extensions.Authentication;
using AccountServer.Extensions.Authorizations;
using CommonLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});

builder.Services.UseControllers();
builder.Services.UseMySql(builder.Configuration.GetConnectionString("AuthDb"));
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString("RedisCache"));
builder.Services.UseHandlers(Assembly.GetExecutingAssembly());
builder.Services.UseMapster(Assembly.GetExecutingAssembly());
builder.Services.UseSessionAuthentication();
builder.Services.UseCredentialAuthentication();
builder.Services.UsePermissionAuthorization();

// TODO 이걸 전부 상속형태로 수정할지 고민
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
