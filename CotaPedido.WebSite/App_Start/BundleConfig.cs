using System.Web;
using System.Web.Optimization;

namespace CotaPedido.WebSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/js/jquery.js",
                        "~/Content/js/bootstrap.js",
                        "~/Content/js/jquery.bxslider.min.js",
                        "~/Content/js/jquery.blImageCenter.js",
                        "~/Content/js/mimity.js",
                        "~/Content/js/bootstrap.touchspin.js",
                        "~/Content/js/jquery.zoom.min.js",
                        "~/Content/js/chosen.jquery.min.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/chosen.min.css",
                      "~/Content/css/jquery.bxslider.css",
                      "~/Content/css/style.css"));

            bundles.Add(new ScriptBundle("~/jqueryval").Include(
                "~/Content/js/jquery.validate.min.js",
                "~/Content/js/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/Cadastro").Include(
                "~/Content/js/jquery.mask.min.js",
                "~/Content/js/Cadastro.js"
                ));

            bundles.Add(new ScriptBundle("~/ListarCompras").Include(
                "~/Content/js/ListarCompras.js"
                ));

            bundles.Add(new ScriptBundle("~/NovaCompra").Include(
                "~/Content/js/chosen.jquery.min.js",
                "~/Content/js/jquery.mask.min.js",
                "~/Content/js/NovaCompra.js"
                ));

            bundles.Add(new ScriptBundle("~/SolicitarPedido").Include(
                "~/Content/js/jquery.mask.min.js",
                "~/Content/js/SolicitarPedido.js"
                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
