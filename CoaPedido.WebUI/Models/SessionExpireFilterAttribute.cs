using PagedList;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CoaPedido.WebUI.Models
{
    public sealed class SessionExpireAttribute : ActionFilterAttribute
    {
        public SessionExpireAttribute(string sessionName)
        {
            SessionName = sessionName;
        }

        public string SessionName { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx != null)
            {                
                if (ctx.Session[SessionName] == null)
                {
                    RouteValueDictionary routesValues = ctx.Request.RequestContext.RouteData.Values;
                    var url = string.Join("/", routesValues.Values.Select(c => c.ToString()));                    
                    filterContext.Result = string.IsNullOrEmpty(url)
                        ? new RedirectResult("~/login")
                        : new RedirectResult("~/login?url=/" + url);
                    
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}