using System.Globalization;

namespace WebApplication4.Middleware;

/// <summary>
/// Проверяет версию клиента по заголовку X-Client-Version.
/// </summary>
public class ClientVersionMiddleware
{
    private const double MinimumVersion = 2.0;

    private readonly RequestDelegate _next;

    public ClientVersionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Client-Version", out var versionValue)
            && double.TryParse(versionValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var version)
            && version < MinimumVersion)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Ошибка: обновите версию ПО");
            return;
        }

        await _next(context);
    }
}
