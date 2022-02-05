using AccountServer.Handlers;
using AccountServer.Models;
using AccountServer.ServiceExtenstions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuthContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Auth"));
});

builder.Services.AddTransientHandler(typeof(IQueryHandler<,>));
builder.Services.AddTransientHandler(typeof(ICommandHandler<>));
builder.Services.AddTransientHandler(typeof(ICommandHandler<,>));
builder.Services.AddTransientHandler(typeof(IRuleChecker<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
