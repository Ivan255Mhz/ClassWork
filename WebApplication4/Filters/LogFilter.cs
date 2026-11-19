using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using WebApplication4.Services;

namespace WebApplication4.Filters;

/// <summary>
/// Пример action-фильтра: пишет имя действия и аргументы в БД.
/// </summary>
public class LogFilter : IAsyncActionFilter
{
    private readonly IDbLogger _logger;

    public LogFilter(IDbLogger logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionName = context.ActionDescriptor.DisplayName ?? "Неизвестное действие";
        await _logger.LogAsync(DbLogLevel.Info, actionName);

        foreach (var argument in context.ActionArguments)
        {
            // CancellationToken не сериализуется в JSON — пропускаем служебные аргументы.
            if (argument.Value is CancellationToken)
            {
                continue;
            }

            string valueJson;

            try
            {
                valueJson = JsonSerializer.Serialize(argument.Value);
            }
            catch (NotSupportedException)
            {
                valueJson = "<значение не сериализуется>";
            }

            await _logger.LogAsync(DbLogLevel.Info, $"{argument.Key}: {valueJson}");
        }

        await next();
    }
}
