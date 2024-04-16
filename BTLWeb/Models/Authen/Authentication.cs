using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BTLWeb.Models.Authen
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Session.GetInt32("UsersId") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Controller","Account"},
                        {"Action","Login"}
                    });
            }
        }
    }
}
