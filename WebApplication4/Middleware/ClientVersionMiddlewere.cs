namespace WebApplication4.Middleware;

public class ClientVersionMiddlewere
{
    private readonly RequestDelegate _next;

    public ClientVersionMiddlewere(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Client-Version", out var versionStr))

        {
            Console.WriteLine("Начало");
            if(double.TryParse(versionStr,System.Globalization.CultureInfo.InvariantCulture ,out double version))
            {
                Console.WriteLine("Распарсили");
                if (version < 2.0)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Ошибка: обновите версию ПО");
                    return;
                }
            }

            await _next(context);
        }  

    }
}
