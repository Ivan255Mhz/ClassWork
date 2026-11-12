using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WebApplication4.Servise;

namespace WebApplication4.Filters;

public class TimingActionFilter : IAsyncActionFilter
{

    private readonly IDbLogger _logger;

    public TimingActionFilter(IDbLogger logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();

        var actionName = context.ActionDescriptor.DisplayName;

        var resultContex = await next();

        sw.Stop();

        await _logger.LogAsync("[FILTER]", $"[FILTER] {actionName} выполнен за {sw.ElapsedMilliseconds} ms");

    }
}
