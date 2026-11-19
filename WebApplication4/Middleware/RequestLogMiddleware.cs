using System.Diagnostics;
using WebApplication4.Services;

namespace WebApplication4.Middleware;

/// <summary>
/// Логирует начало и конец каждого запроса в БД.
/// </summary>
public class RequestLogMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IDbLogger dbLogger)
    {
        var stopwatch = Stopwatch.StartNew();

        await dbLogger.LogAsync(
            DbLogLevel.Info,
            $"Запрос {context.Request.Method} {context.Request.Path}");

        await _next(context);

        stopwatch.Stop();

        await dbLogger.LogAsync(
            DbLogLevel.Info,
            $"Ответ {context.Response.StatusCode} за {stopwatch.ElapsedMilliseconds} мс");
    }
}
