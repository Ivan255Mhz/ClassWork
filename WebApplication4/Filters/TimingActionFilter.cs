using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication4.Services;

namespace WebApplication4.Filters;

/// <summary>
/// Глобальный фильтр: измеряет время выполнения действия и пишет его в БД.
/// </summary>
public class TimingActionFilter : IAsyncActionFilter
{
    private readonly IDbLogger _logger;

    public TimingActionFilter(IDbLogger logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        var actionName = context.ActionDescriptor.DisplayName ?? "Неизвестное действие";

        await next();

        stopwatch.Stop();

        await _logger.LogAsync(
            DbLogLevel.Info,
            $"{actionName} выполнен за {stopwatch.ElapsedMilliseconds} мс");
    }
}
