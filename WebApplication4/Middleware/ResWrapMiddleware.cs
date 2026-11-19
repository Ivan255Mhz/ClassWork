using System.Text;
using System.Text.Json;

namespace WebApplication4.Middleware;

/// <summary>
/// Оборачивает JSON-ответ в конверт { data, serverTime, version }.
/// </summary>
public class ResWrapMiddleware
{
    private readonly RequestDelegate _next;

    public ResWrapMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;

        try
        {
            using var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await _next(context); // исключение отсюда НЕ ловим — пусть идёт выше

            var isJson = context.Response.ContentType is not null
                         && context.Response.ContentType.Contains("application/json");

            if (isJson
                && context.Response.StatusCode == StatusCodes.Status200OK
                && !context.Response.HasStarted)
            {
                buffer.Position = 0;
                var body = await new StreamReader(buffer, Encoding.UTF8).ReadToEndAsync();

                context.Response.Body = originalBody;
                await context.Response.WriteAsync(WrapJson(body), Encoding.UTF8);
            }
            else
            {
                context.Response.Body = originalBody;
                buffer.Position = 0;
                await buffer.CopyToAsync(originalBody);
            }
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }

    private static string WrapJson(string originalJson)
    {
        try
        {
            var json = JsonSerializer.Deserialize<JsonElement>(originalJson);

            var wrapped = new
            {
                data = json,
                serverTime = DateTime.UtcNow.ToString("O"),
                version = "1.0"
            };

            return JsonSerializer.Serialize(wrapped);
        }
        catch
        {
            return originalJson; // это не JSON — отдаём без изменений
        }
    }
}
