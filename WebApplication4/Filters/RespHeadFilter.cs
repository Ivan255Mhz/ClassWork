using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication4.Filters;

public class RespHeadFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context) { }
    

    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.Append("X-Toy", "1.000");
    }
}
