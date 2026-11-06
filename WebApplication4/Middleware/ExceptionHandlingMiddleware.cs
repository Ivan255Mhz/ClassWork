using System.Net;
using WebApplication4.Servise;
using System.Text.Json;

namespace WebApplication4.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IDbLogger dbLogger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await dbLogger.LogAsync(Servise.LogLevel.Error, ex.Message, ex.StackTrace);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new { Error = "Внутренняя ошибка сервера. Мы работаем над этим" };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));

        }
    }
}
