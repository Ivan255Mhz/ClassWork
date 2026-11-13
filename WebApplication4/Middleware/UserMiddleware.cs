namespace WebApplication4.Middleware;

public class UserMiddleware
{
    private readonly RequestDelegate _next;

    public UserMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        var c = context.User.Claims;

        foreach (var u in c)
        {
            Console.WriteLine(u);
        }

        await _next(context);
    }
}
