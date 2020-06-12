using System.Web;
using System.Web.Mvc;
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
                    filterContext.Result = new RedirectResult("~/");
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}