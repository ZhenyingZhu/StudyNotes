# Logging

## OpenTelemetry

<https://opentelemetry.io/>

- observability
- traces, metrics, or logs

[ETW](https://learn.microsoft.com/en-us/windows-hardware/drivers/devtest/event-tracing-for-windows--etw-)

[OTEL Console](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/logs/getting-started-console/README.md)

- Add OpenTelemetry as a logging provider.
- instantiating a LoggerFactory instance
- create a logging pipeline
- this is to console, which is not for prod

[Compile-time logging source generation](https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator)

- `LoggerMessageAttribute`: `[LoggerMessage()]`

[OTEL .Net Core](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/logs/getting-started-aspnetcore/README.md)

- OTLP Exporter is used. Paired with a batch processor
- `app.Logger.StartingApp();`

Need to understand the builder pattern

```C#
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddOpenTelemetry(logging =>
    {
        logging.AddConsoleExporter();
    });
});
```

Using builder pattern.
