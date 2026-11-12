using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication4.Filters;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(!context.HttpContext.Request.Headers.ContainsKey("X-Api-Key"))
        {
            context.Result = new UnauthorizedObjectResult( new { Error ="Нет Api ключа"});
        }
    }
}
