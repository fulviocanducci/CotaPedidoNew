using CotaPedido.WebUI.Models;
using System.Web.Mvc;

namespace CoaPedido.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //tEM QUE MUDA AQUI PARA LISTAR APENAS OS PEDIDOS PUBLICADOS.

            var model = new CotacaoModel();
            model.ExibirPedidosHome();


            //var model = new CotacaoModel();
            model.CarregarTodasCotacoes();
            model.Categorias = model.ObterCategorias();

            return View(model);
        }

        //public ActionResult ListarCotacoesCategoria(int idCategoria, int idSubCategoria)
        //{
        //    var model = new CotacaoModel();

        //    //model.CarregarCotacoesCategoria(idCategoria, idSubCategoria);
        //    //Alterado por Sérgio em 15/01/2018 -> para listar os pedidos sem a necessidade de ter cotações, filtrando pela sua categoria e subcategoria.
        //    model.ExibirPedidosHomePorCategorias(idCategoria, idSubCategoria);
        //    model.Categorias = model.ObterCategorias();

        //    return View(model);
        //}

        //Change Fúlvio
        [Route("pedidos/{idCategoria}/{idSubCategoria}", Order = 1, Name = "RouteListaCategoriaSubCategoria")]
        public ActionResult ListarPedidosCategoria(string idCategoria, string idSubCategoria)
        {
            var model = new CotacaoModel();
            model.ExibirPedidosHomePorCategorias(idCategoria, idSubCategoria);
            model.Categorias = model.ObterCategorias();

            ViewBag.IdCategoria = idCategoria;
            ViewBag.IdSubCategoria = idSubCategoria;

            return View("Index", model);
        }

        [HttpGet]
        [ValidateInput(true)]
        [Route("pedidos/busca", Order = 2, Name = "RouteListaCategoriaSubCategoriaItensDescricao")]
        public ActionResult ListarPedidosCategoriaItensDescricao(string descricao)
        {
            if (string.IsNullOrEmpty(descricao))
            {
                return RedirectToAction("Index");
            }
            var model = new CotacaoModel();
            model.ExibirPedidosHomePorDescricaoPedido(descricao);
            model.Categorias = model.ObterCategorias();
            //ViewBag.IdCategoria = idCategoria;
            //ViewBag.IdSubCategoria = idSubCategoria;
            ViewBag.Descricao = descricao;
            return View("Index", model);
        }
                

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    string fileName = Server.MapPath("~/Content/data.xlsx");
        //    ImportFromExcel import = new ImportFromExcel(fileName);

        //    var result = import.Render().ToList();
        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ExportToExcel export = new ExportToExcel("Plan1");
        //    var result = export.Render(
        //        new[] 
        //        {
        //            new { Item = 1, Produto = "Produto 1", Unidade = "Peças", Quantidade = 152, ValorUnitario = 125d },
        //            new { Item = 2, Produto = "Produto 2", Unidade = "Peças", Quantidade = 155, ValorUnitario = 350.6d }
        //        }
        //        .ToArray());

        //    return File(result, "application/vnd.ms-excel", string.Format("Teste_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
        //}
    }
}