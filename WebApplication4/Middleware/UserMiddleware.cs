using System.Security.Claims;

namespace WebApplication4.Middleware;

/// <summary>
/// Пишет claims текущего пользователя в лог (учебный пример).
/// </summary>
public class UserMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserMiddleware> _logger;

    public UserMiddleware(RequestDelegate next, ILogger<UserMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        foreach (Claim claim in context.User.Claims)
        {
            _logger.LogInformation("Claim: {Type} = {Value}", claim.Type, claim.Value);
        }

        await _next(context);
    }
}
