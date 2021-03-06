using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
//
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
//
app.UseAuthorization();

app.MapControllers();

app.Run();
