using System.Web.Mvc;

namespace CoaPedido.WebUI.Controllers
{
    public class SiteController : Controller
    {        
        [Route("como-funciona", Name = "SiteComoFunciona")]
        public ActionResult ComoFunciona()
        {
            return View();
        }

        [Route("faq", Name = "SiteFaq")]
        public ActionResult Faq()
        {
            return View();
        }

        [Route("sobre", Name = "SiteSobre")]
        public ActionResult Sobre()
        {
            return View();
        }

        [Route("politica-de-uso", Name = "SitePoliticaUso")]
        public ActionResult PoliticaUso()
        {
            return View();
        }

        [Route("termo-de-uso", Name = "SiteTermoUso")]
        public ActionResult TermoUso()
        {
            return View();
        }

    }
}