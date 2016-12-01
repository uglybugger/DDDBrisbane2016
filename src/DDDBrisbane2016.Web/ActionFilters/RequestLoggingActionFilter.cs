using System.Web.Mvc;

namespace DDDBrisbane2016.Web.ActionFilters
{
    public class RequestLoggingActionFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}