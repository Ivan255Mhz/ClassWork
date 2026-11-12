using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication4.Data.Dtos;

namespace WebApplication4.Filters;

public class AntiHackerFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context) { }
    

    public void OnActionExecuting(ActionExecutingContext context)
    {
        

        if (context.ActionArguments.TryGetValue("dto", out var arg)
            && arg is CreateToyDto dto)
        {
            
            if (dto.Name.Contains("Хакер"))
            {
                context.Result = new BadRequestObjectResult("Запрещеное слово");
            }
        }
    }
}
