using System.Diagnostics;
using WebApplication4.Servise;


namespace WebApplication4.Middleware;

public class RequestLogMiddleware
{
    private readonly RequestDelegate _next;
    

    public RequestLogMiddleware(RequestDelegate next)
    {
        _next = next;
        
    }

    public async Task InvokeAsync(HttpContext context,IDbLogger dbLogger)
    {
       var stopwatch = Stopwatch.StartNew();

        var message = $"Запрос {context.Request.Method} , {context.Request.Path}";
         await dbLogger.LogAsync(Servise.LogLevel.Info, message);

        await _next(context);
        stopwatch.Stop();

        message = $"Ответ : {context.Response.StatusCode} за {stopwatch.ElapsedMilliseconds} мс.";
        await dbLogger.LogAsync(Servise.LogLevel.Info, message);
    }
}
