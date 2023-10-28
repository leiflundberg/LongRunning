using Serilog;
using Serilog.Context;

namespace Test
{
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

            var correlationId = Guid.NewGuid();
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                for (int id = 0; id < 10; id++)
                {
                    Log.Information($"this is a log message with a different id: {id}.");
                }
            }
        }
    }
}