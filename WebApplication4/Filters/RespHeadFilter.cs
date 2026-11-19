using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication4.Filters;

/// <summary>
/// Пример result-фильтра: добавляет собственный заголовок к ответу.
/// </summary>
public class RespHeadFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Append("X-Toy", "1.000");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
