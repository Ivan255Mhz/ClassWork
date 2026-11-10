namespace WebApplication4.Middleware;

public class ServerTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ServerTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Server-Time", DateTime.UtcNow.ToString("o"));
        await _next(context);
    }
}
