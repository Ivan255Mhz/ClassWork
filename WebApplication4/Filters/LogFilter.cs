using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using WebApplication4.Servise;

namespace WebApplication4.Filters;

public class LogFilter : IAsyncActionFilter
{
    private readonly IDbLogger _logger;

    public LogFilter(IDbLogger logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var Name = context.ActionDescriptor.DisplayName;
        await _logger.LogAsync("[]", Name);

        foreach (var agr in context.ActionArguments)
        {

            var valJson = JsonSerializer.Serialize(agr.Value);

             await _logger.LogAsync("[]", $"{agr.Key}: {valJson}");

        }

        await next();
    }
}
