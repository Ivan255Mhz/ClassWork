using System.Diagnostics;

namespace WebApplication4.Middleware;

/// <summary>
/// Добавляет к ответу заголовок X-Time-Header со временем обработки запроса.
/// </summary>
public class TimingHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public TimingHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            context.Response.Headers.Append("X-Time-Header", stopwatch.ElapsedMilliseconds.ToString());
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
