using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CotaPedido.WebSite.Controllers
{
    public class LogoutController : Controller
    {
        //
        // GET: /Logout/
        public ActionResult Index()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
	}
}