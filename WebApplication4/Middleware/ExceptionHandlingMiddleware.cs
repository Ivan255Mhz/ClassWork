using System.Net;
using System.Text.Json;
using WebApplication4.Services;

namespace WebApplication4.Middleware;

/// <summary>
/// Перехватывает необработанные исключения и возвращает JSON-ответ.
/// </summary>
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
            await dbLogger.LogAsync(DbLogLevel.Error, ex.Message, ex.StackTrace);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new { Error = "Внутренняя ошибка сервера. Мы работаем над этим" };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
