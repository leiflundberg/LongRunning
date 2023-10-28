using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;

class CustomJsonFormatter : ITextFormatter
{
    private static readonly Dictionary<LogEventLevel, string> LogLevelStrings = new()
    {
        { LogEventLevel.Verbose, "Verbose" },
        { LogEventLevel.Debug, "Debug" },
        { LogEventLevel.Information, "Information" },
        { LogEventLevel.Warning, "Warning" },
        { LogEventLevel.Error, "Error" },
        { LogEventLevel.Fatal, "Fatal" }
    };

    public void Format(LogEvent logEvent, TextWriter output)
    {
        string correlationId = "";
        var correlationIdProperty = logEvent.Properties.ContainsKey("CorrelationId") ? logEvent.Properties["CorrelationId"].ToString() : null;
        if (correlationIdProperty != null)
        {
            correlationId = correlationIdProperty;
        }

        var json = new StringWriter();
        using (var jsonWriter = new JsonTextWriter(json))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("timestamp");
            jsonWriter.WriteValue(logEvent.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss"));
            jsonWriter.WritePropertyName("level");
            jsonWriter.WriteValue(LogLevelStrings[logEvent.Level]);
            jsonWriter.WritePropertyName("message");
            jsonWriter.WriteValue(logEvent.RenderMessage());
            jsonWriter.WritePropertyName("correlationId");
            jsonWriter.WriteValue(correlationId);
            jsonWriter.WriteEndObject();
        }
        output.Write(json.ToString() + "\n");
    }
}
