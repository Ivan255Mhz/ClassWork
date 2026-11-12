using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication4.Filters;

public class ToyExFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is ArgumentException argEx)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Error = "Неверный аргумент",
                Msg = argEx.Message
            });
        }
        context.ExceptionHandled = true;
    }
}
