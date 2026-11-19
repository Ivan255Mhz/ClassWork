using System;
using WebApplication4.Servise;

namespace WebApplication4.Middleware;

public class AuditBodyMiddlewere
{
    private readonly RequestDelegate _next;

    public AuditBodyMiddlewere(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context , IDbLogger logger)
    {
        Console.WriteLine("Начало");
        context.Request.EnableBuffering();
        Console.WriteLine("Создали буфер");
        var reader = new StreamReader(context.Request.Body);
        
        var bodyStr =await reader.ReadToEndAsync();
        Console.WriteLine("Прочитали");
        context.Request.Body.Position = 0;
        Console.WriteLine("Сдвинули в 0");

        if (context.Request.Method == "POST" || context.Request.Method == "DELETE")
        {
            Console.WriteLine("Прошли условие 1");
            if (!string.IsNullOrWhiteSpace(bodyStr))
            {
                Console.WriteLine("Отработано тело " + bodyStr);
                 await logger.LogAsync("[INFO]",bodyStr);
            }
        }

        await _next(context);

    }
}
