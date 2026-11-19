using WebApplication4.Services;

namespace WebApplication4.Middleware;

/// <summary>
/// Пишет тело POST/DELETE-запросов в журнал БД.
/// </summary>
public class AuditBodyMiddleware
{
    private readonly RequestDelegate _next;

    public AuditBodyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IDbLogger logger)
    {
        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        if ((context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Delete)
            && !string.IsNullOrWhiteSpace(body))
        {
            await logger.LogAsync(DbLogLevel.Info, body);
        }

        await _next(context);
    }
}
