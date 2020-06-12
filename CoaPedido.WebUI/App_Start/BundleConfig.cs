using System.Web;
using System.Web.Optimization;

namespace CoaPedido.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery-3.4.1.js",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/jqueryval2").Include(                
                        "~/Scripts/validation/jquery.validate*"));
                        
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.touchspin.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/select2.min.js",
                      "~/Scripts/jquery.scrollUp.min.js",
                      "~/Scripts/price-range.js",
                      "~/Scripts/jquery.prettyPhoto.js",
                      "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/scriptsIE9").Include(
                      "~/Scripts/html5shiv.js",
                      "~/Scripts/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/prettyPhoto.css",
                        "~/Content/css/price-range.css",
                        "~/Content/css/animate.css",
                        "~/Content/css/main.css",
                        "~/Scripts/dropzone/css/basic.css",
                        "~/Scripts/dropzone/css/dropzone.css",
                        "~/Content/css/responsive.css",
                        "~/Content/css/select2.css",
                        "~/Content/css/select2-bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css-new").Include(
                        "~/Content/css/slick/slick.css",
                        "~/Content/css/slick/slick-theme.css", 
                        "~/Content/css/estilos.css",
                        "~/Content/css/fontawesome/css/all.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/js-new").Include(
                "~/Scripts/js/jquery-3.5.1.min.js",
                "~/Scripts/js/main.js",
                "~/Scripts/js/slick/slick.min.js"));

            bundles.Add(new ScriptBundle("~/NovaCompra").Include(
                "~/Scripts/chosen.jquery.min.js",
                "~/Scripts/jquery.mask.min.js",
                "~/Scripts/dropzone/dropzone.js",
                "~/Scripts/NovaCompra.js"
                ));

            bundles.Add(new ScriptBundle("~/Cadastro").Include(
               "~/Scripts/jquery.mask.min.js",
               "~/Scripts/Cadastro.js",
               "~/Scripts/validation/jquery.validate.min.js",
               "~/Scripts/validation/jquery.validate.unobtrusive.min.js"
               ));

            bundles.Add(new ScriptBundle("~/ListarCompras").Include(
                "~/Scripts/ListarCompras.js"
                ));

            bundles.Add(new ScriptBundle("~/SolicitarPedido").Include(
                "~/Scripts/jquery.mask.min.js",
                "~/Scripts/SolicitarPedido.js"
                ));

            bundles.Add(new ScriptBundle("~/Cotar").Include(
                "~/Scripts/chosen.jquery.min.js",
                "~/Scripts/jquery.mask.min.js",
                "~/Scripts/Cotar.js"
                ));
            
            bundles.Add(new ScriptBundle("~/ExportJs").Include(
                "~/Scripts/tableExport.js",
                "~/Scripts/jquery.base64.js"
                ));

            bundles.Add(new ScriptBundle("~/CompararCotacoes").Include(
                "~/Scripts/CompararCotacoes.js"
                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
