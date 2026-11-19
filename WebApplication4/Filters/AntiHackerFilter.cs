using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication4.Data.Dtos;

namespace WebApplication4.Filters;

/// <summary>
/// Пример action-фильтра: блокирует недопустимые слова в названии игрушки.
/// </summary>
public class AntiHackerFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("dto", out var argument)
            && argument is CreateToyDto dto
            && dto.Name.Contains("Хакер", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new BadRequestObjectResult("Запрещённое слово в названии");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
