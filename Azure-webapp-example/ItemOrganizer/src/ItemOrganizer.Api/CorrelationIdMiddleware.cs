namespace ItemOrganizer.Api;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var requestedId = context.Request.Headers[HeaderName].FirstOrDefault();
        context.TraceIdentifier = IsValid(requestedId)
            ? requestedId!
            : Guid.NewGuid().ToString("N");
        context.Response.Headers[HeaderName] = context.TraceIdentifier;

        await next(context);
    }

    private static bool IsValid(string? value)
    {
        return value is { Length: > 0 and <= 100 }
            && value.All(character =>
                char.IsLetterOrDigit(character) || character is '-' or '_' or '.');
    }
}
