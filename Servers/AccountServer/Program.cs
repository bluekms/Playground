using AccountServer.Features;
using AccountServer.Models;
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

var registrant = new HandlerRegistrant(builder.Services);
registrant.Register(typeof(IQueryHandler<,>));
registrant.Register(typeof(ICommandHandler<>));
registrant.Register(typeof(ICommandHandler<,>));
registrant.Register(typeof(IRuleChecker<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
