using LongRunning;
using Serilog;

Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console(new CustomJsonFormatter())
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
