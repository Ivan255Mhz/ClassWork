using System.Diagnostics;

namespace WebApplication4.Middleware;

public class TimingHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public TimingHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            sw.Stop();
            context.Response.Headers.Append("X-Time-Header", sw.ElapsedMilliseconds.ToString());
            return Task.CompletedTask;
        });
        await _next(context);
    }
}
