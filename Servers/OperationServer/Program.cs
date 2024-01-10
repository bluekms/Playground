using System.Reflection;
using AuthDb;
using AuthLibrary.Extensions;
using AuthLibrary.Extensions.Authentication;
using AuthLibrary.Extensions.Authorizations;
using AuthLibrary.Models;
using CommonLibrary;
using CommonLibrary.Extensions;
using CommonLibrary.Handlers;
using Protobuf.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLogger();
builder.Host.UseStashbox();
builder.Services.UseNginx();
builder.Services.UseRedisCache(builder.Configuration.GetConnectionString(RedisCacheExtension.ConfigurationSection)!);
builder.Services.UsePostgreSql<AuthDbContext, ReadOnlyAuthDbContext>(
    builder.Configuration.GetConnectionString(AuthDbContext.ConfigurationSection),
    "AuthServer");
builder.Services.UseMapster();

// builder.Services.UseSessionAuthentication();
// builder.Services.UseCredentialAuthentication();
// builder.Services.UseOpenAuthentication();
// builder.Services.UsePermissionAuthorization();
//
builder.Services.UseHandlers(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(AssemblyEntry))!);
builder.Services.UseProtobuf();
builder.Services.AddRazorPages();

// 추가 DI
builder.Services.AddScoped<ITimeService, ScopedTimeService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
