using System.Reflection;
using AuthDb;
using CommonLibrary;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Logger

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});

// Add Controllers

builder.Services.AddControllers();

// Add Database

builder.Services.AddDbContext<AuthContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Auth"));
});

// Add Transient

builder.Services.AddHandlers(Assembly.GetExecutingAssembly());

builder.Services.AddMapster(config =>
{
    config.RequireDestinationMemberSource = true;
    config.Default.MapToConstructor(true);
});
builder.Services.AddMapsterRegisters(Assembly.GetExecutingAssembly());

// Add Scoped

builder.Services.AddScoped<ITimeService, ScopedTimeService>();

// Configure the HTTP request pipeline.

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
