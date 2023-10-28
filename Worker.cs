using Serilog;

namespace LongRunning;

public class Worker : BackgroundService
{
    public Worker() { }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Log.Information("Main thread worker.");
            Thread pollingThread = new(new ThreadStart(HttpPolling));
            pollingThread.Start();
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    public static void HttpPolling() 
    {
        HttpClient client = new();
        var response = client.GetAsync("https://www.google.com");
        if (response.Result.IsSuccessStatusCode)
        {
            Log.Information("Google is up, can you believe it?");
        }
        else
        {
            Log.Information("Not happening");
        }
    }
}
