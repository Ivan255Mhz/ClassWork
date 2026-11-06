using System.Text;
using System.Text.Json;

namespace WebApplication4.Middleware;

public class ResWrapMiddleware
{
    private readonly RequestDelegate _next;

    public ResWrapMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;

        try
        {
            using var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await _next(context); // исключение отсюда НЕ ловим — пусть идёт выше

            var isJson = context.Response.ContentType is not null &&
                         context.Response.ContentType.Contains("application/json");

            if (isJson &&
                context.Response.StatusCode == StatusCodes.Status200OK &&
                !context.Response.HasStarted)
            {
                buffer.Position = 0;
                var body = await new StreamReader(buffer, Encoding.UTF8).ReadToEndAsync();

                context.Response.Body = originalBody;
                await context.Response.WriteAsync(WrapJson(body), Encoding.UTF8);
            }
            else
            {
                // отдаём ответ как есть
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

    private string WrapJson(string origJson)
    {
        try
        {
            var je = JsonSerializer.Deserialize<JsonElement>(origJson);

            var wrap = new
            {
                data = je,
                serverTime = DateTime.UtcNow.ToString("O"),
                version = "1.0"
            };

            return JsonSerializer.Serialize(wrap);
        }
        catch
        {
            return origJson; // это не JSON — отдаём без изменений
        }
    }
}
