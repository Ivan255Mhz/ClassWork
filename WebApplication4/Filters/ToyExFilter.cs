using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication4.Filters;

/// <summary>
/// Пример exception-фильтра: превращает ArgumentException в ответ 400.
/// </summary>
public class ToyExFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ArgumentException argumentException)
        {
            return;
        }

        context.Result = new BadRequestObjectResult(new
        {
            Error = "Неверный аргумент",
            Msg = argumentException.Message
        });

        context.ExceptionHandled = true;
    }
}
