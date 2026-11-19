using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication4.Filters;

/// <summary>
/// Пример authorization-фильтра: требует заголовок X-Api-Key.
/// </summary>
public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private const string HeaderName = "X-Api-Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var apiKey)
            || string.IsNullOrWhiteSpace(apiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { Error = "Нет API-ключа" });
        }
    }
}
