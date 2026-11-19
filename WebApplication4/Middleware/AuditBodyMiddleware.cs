using WebApplication4.Services;

namespace WebApplication4.Middleware;

/// <summary>
/// Пишет текстовое тело POST/DELETE-запросов в журнал БД.
/// Бинарные тела (загрузка файлов) не логируем.
/// </summary>
public class AuditBodyMiddleware
{
    private const int MaxBodyLength = 4000;

    private readonly RequestDelegate _next;

    public AuditBodyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IDbLogger logger)
    {
        if (!ShouldLogBody(context.Request))
        {
            await _next(context);
            return;
        }

        context.Request.EnableBuffering();

        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        if (!string.IsNullOrWhiteSpace(body))
        {
            if (body.Length > MaxBodyLength)
            {
                body = body[..MaxBodyLength] + "... (обрезано)";
            }

            await logger.LogAsync(DbLogLevel.Info, body);
        }

        await _next(context);
    }

    private static bool ShouldLogBody(HttpRequest request)
    {
        var isWriteMethod =
            HttpMethods.IsPost(request.Method) ||
            HttpMethods.IsDelete(request.Method);

        if (!isWriteMethod)
        {
            return false;
        }

        var contentType = request.ContentType ?? string.Empty;

        return contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)
            || contentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
    }
}
