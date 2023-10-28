using Serilog;
using Test;

Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console(new CustomJsonFormatter())
    .CreateLogger();
    
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(); // <- the magical method
builder.Services.AddHostedService<Worker>();
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
