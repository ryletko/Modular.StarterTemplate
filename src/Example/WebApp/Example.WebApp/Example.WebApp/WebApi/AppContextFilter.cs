using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Modular.Framework.Infrastructure.AppContext;
using AppContext = Modular.Framework.Infrastructure.AppContext.AppContext;

namespace Example.WebApp.WebApi;

public class AppContextFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        AppContextAccessor.Current = new AppContext(userId, DateTimeOffset.Now);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}