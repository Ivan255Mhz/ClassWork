namespace WebApplication4.Middleware;

public class SecurityHeadersMiddleware
{

    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Append("X-Content-Type-Options","nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        await _next(context);
    }

}
