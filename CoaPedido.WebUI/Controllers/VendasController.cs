using CoaPedido.WebUI.Models;
using CotaPedido.Entidades;
using CotaPedido.Excel;
using CotaPedido.Infra.Repositorios.SQLServer;
using CotaPedido.WebUI.Models;
using NPOI.SS.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoaPedido.WebUI.Controllers
{    
    public class VendasController : Controller
    {
        [SessionExpire("userVendedor")]
        public ActionResult Index(CotacaoModel model, int? page)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            model.IdVendedor = vendedor.VendedorId;
            model.CarregarCotacoes(page);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ListaCotacoes", model)
                : View(model);
        }

        [SessionExpire("userVendedor")]
        public ActionResult Pedidos(CotacaoModel model, int? page)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            model.IdVendedor = vendedor.VendedorId;
            model.CarregarPedidos(page);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ListaPedidos", model)
                : View(model);
        }

        [HttpGet]
        [Route("vendas-detalhes-pedido/{name}-{id}", Name = "VendasDetalhesPedido2")]
        [Route("vendas/detalhespedido/{id}", Name = "VendasDetalhesPedido1")]
        [Route("cotacao/{id}/{name}", Name = "VendasDetalhesPedido")]
        public ActionResult DetalhesPedido(int? id)
        {
            SessionVendedor vendedor = RecuperarVendedor();
            /* --Desabilitado por Sérgio para permitir que se visualiza o pedido por fora do site.
            /*if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", new { link = Request.Url.PathAndQuery });
            }
            */
            var model = new CotacaoModel();
            model.CarregarPedido(id.Value);

            return View(model);
        }
        
        [HttpGet]
        [SessionExpire("userVendedor")]
        public ActionResult Cotar(int? id)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }
            var model = new CotacaoModel();
            model.CarregarPedido(id.Value);
            model.CarregaItens(model.Pedido.Itens);
            model.IdPedido = id.Value;
            model.IdVendedor = vendedor.VendedorId;
            model.DataCotacao = DateTime.Now.ToString("dd/MM/yyyy");
            return View(model);
        }

        [HttpPost]
        [SessionExpire("userVendedor")]
        public ActionResult Cotar(CotacaoModel model)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.EmailVendedor = vendedor.Email;
                    model.NomeVendedor = vendedor.NomeFantasia;
                    model.InserirCotacao();

                    TempData.Add("CotacaoCadastrada", true);

                    return RedirectToAction("Index", "Vendas");
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar gravar cotação");
                }
            }

            model.CarregarPedido(model.IdPedido);

            return View(model);
        }

        [HttpGet]
        [SessionExpire("userVendedor")]
        public ActionResult EditarCotacao(int id)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            var model = new CotacaoModel();
            model.CarregarCotacao(id);
            if (TempData.ContainsKey("Status"))
            {
                ViewBag.Status = TempData["Status"];
            }
            return View(model);
        }

        [HttpPost]
        [SessionExpire("userVendedor")]
        public ActionResult EditarCotacao(CotacaoModel model)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.EmailVendedor = vendedor.Email;
                    model.NomeVendedor = vendedor.NomeFantasia;
                    model.EditarCotacao();

                    TempData.Add("CotacaoAlterada", true);

                    return RedirectToAction("Index", "Vendas");
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar gravar cotação");
                }
            }

            model.CarregarPedido(model.IdPedido);

            return View(model);
        }

        [HttpGet]
        [SessionExpire("userVendedor")]
        [Route("vendas-visualizar-cotacao/{name}-{id}", Name = "VendasVisualizarCotacao")]
        public ActionResult VisualizarCotacao(int id)
        {
            var model = new CotacaoModel();

            model.CarregarCotacao(id);   //Tem que mudar aqui para Visualizar o pedido e não pela cotação.

            return View(model);
        }

        [HttpGet]
        [SessionExpire("userVendedor")]
        public FileContentResult ExportarItens(int id)
        {            
            var model = new CotacaoModel();

            model.CarregarCotacao(id);

            ExportToExcel export = new ExportToExcel("Plan1");

            string[] headers = { "Id", "Item", "Seq.", "Produto", "Unidade", "Quantidade", "Valor Unitário" };
            string[] items = { "Id", "IdItem", "Seq", "NomeProduto", "Unidade", "Quantidade", "ValorUnitario" };
            int[] widths = { 1, 1, 3000, 10000, 3000, 3250, 4100 };
            string[] cellTypesParse = { "i", "i", "i", "s", "s", "d", "d" };
            CellType[] cellTypes = { CellType.Numeric, CellType.Numeric, CellType.Numeric, CellType.String, CellType.String, CellType.Numeric, CellType.Numeric };
            string name = string.Format("Cotacao_Itens_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));

            var listOfItems = model.Itens
                .Select((x, y) => new
                {                    
                    x.Id,
                    x.IdItem,
                    Seq = (y + 1),
                    x.NomeProduto,
                    Unidade = x.Unidade.Descricao,
                    x.Quantidade,
                    x.ValorUnitario
                })
                .ToList();
            
            var result = export
                .SetCellTypesParse(cellTypesParse)
                .SetWidths(widths)
                .SetTypes(cellTypes)
                .Header(headers)                
                .Render(listOfItems, items);

            return File(result.ToArray(), "application/vnd.ms-excel", name);
        }

        [SessionExpire("userVendedor")]
        public ActionResult ImportarItens(HttpPostedFileBase arquivo, int IdCotacao, FormCollection form)
        {
            RepositorioItemCotacao repositorio = null;
            ImportFromExcel importFromExcel = null;
            try
            {
                if (arquivo.InputStream.Length > 0)
                {
                    repositorio = new RepositorioItemCotacao();
                    importFromExcel = new ImportFromExcel();
                    var vendedor = RecuperarVendedor();
                    arquivo.InputStream.Position = 0;
                    var dados = importFromExcel.Render(arquivo.InputStream).ToList();
                    if (dados.Count > 0)
                    {
                        var results = repositorio.UpdateFromExcel(dados).ToList();
                        TempData["Status"] = "Itens da Cotação alterados";
                    }
                    else
                    {
                        TempData["Status"] = "Não houve alteração dados conflitantes";
                    }
                    repositorio = null;
                    importFromExcel = null;
                }
            }
            catch (Exception)
            {
                TempData["Status"] = "Problemas no processamento, entre em contato com administrador";
            }
            finally
            {
                repositorio = null;
                importFromExcel = null;
            }
            return RedirectToAction("EditarCotacao", new { Id = IdCotacao });
        }

        #region oldImportItens

        //[HttpPost]
        //public ActionResult ImportarItens(HttpPostedFileBase arquivo, int IdCotacao, FormCollection form)
        //{
        //    string pathSave = "";
        //    FileStream fileStream = null;
        //    try
        //    {            
        //        var vendedor = RecuperarVendedor();
        //        var namePathRaiz = "~/Temp/";
        //        if (!Directory.Exists(Server.MapPath(namePathRaiz)))
        //            Directory.CreateDirectory(Server.MapPath(namePathRaiz));

        //        if (arquivo != null && !string.IsNullOrEmpty(arquivo.FileName) && arquivo.ContentLength > 0)
        //        {
        //            pathSave = Server.MapPath(namePathRaiz) +  Guid.NewGuid().ToString() + arquivo.FileName;
        //            arquivo.SaveAs(pathSave);
        //            using (fileStream = System.IO.File.Open(pathSave, FileMode.Open, FileAccess.Read))
        //            {
        //                RepositorioItemCotacao repositorio = new RepositorioItemCotacao();
        //                ImportFromExcel importFromExcel = new ImportFromExcel();
        //                fileStream.Position = 0;
        //                var dados = importFromExcel.Render(fileStream).ToList();
        //                if (dados.Count > 0)
        //                {
        //                    var results = repositorio.UpdateFromExcel(dados).ToList();
        //                    TempData["Status"] = "Itens da Cotação alterados";
        //                }
        //                else
        //                {
        //                    TempData["Status"] = "Não houve alteração dados conflitantes";
        //                }
        //                repositorio = null;
        //                importFromExcel = null;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData["Status"] = "Problemas no processamento, entre em contato com administrador";                
        //    }
        //    if (fileStream != null)
        //    {                
        //        fileStream.Dispose();
        //    }
        //    if (System.IO.File.Exists(pathSave))
        //    {
        //        //System.IO.File.Delete(pathSave);
        //    }
        //    return RedirectToAction("EditarCotacao", new { Id = IdCotacao });

        //    #region error_code
        //    //if (vendedor == null)
        //    //{
        //    //    return RedirectToAction("Vendedor", "Cadastro", null);
        //    //}

        //    //try
        //    //{
        //    //    if (model.Arquivo != null && model.Arquivo.ContentLength > 0)
        //    //    {
        //    //        var tempFolder = vendedor.NomeFantasia;
        //    //        if (!Directory.Exists(Server.MapPath("~/Temp/" + tempFolder)))
        //    //            Directory.CreateDirectory(Server.MapPath("~/Temp/" + tempFolder));

        //    //        //Save file content goes here
        //    //        var pathString = Path.Combine(Server.MapPath("~/Temp/" + tempFolder), Path.GetFileName(model.Arquivo.FileName));
        //    //        model.Arquivo.SaveAs(pathString);

        //    //        model.LerArquivoItensCotacao(pathString);
        //    //    }
        //    //    else
        //    //    {
        //    //        return Json(new { result = "Erro", erro = "Selecione um arquivo para importar" });
        //    //    }
        //    //}
        //    //catch (InvalidOperationException ex)
        //    //{
        //    //    return Json(new { result = "Erro", erro = ex.Message });
        //    //}
        //    //catch
        //    //{
        //    //    return Json(new { result = "Erro", erro = "Erro ao importar arquivo" });
        //    //}
        //    //finally
        //    //{
        //    //    RemoveAllTempFiles();
        //    //}

        //    //TempData.Add("CotacaoCadastrada", true);

        //    //return Json(new { result = "Redirect", url = Url.Action("Index", "Vendas") });
        //    #endregion
        //}
        #endregion

        #region Métodos Privados

        private void RemoveAllTempFiles()
        {
            if (Directory.Exists(Server.MapPath("~/Temp/")))
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/Temp/"));

                foreach (var subDirectory in dir.GetDirectories())
                {
                    subDirectory.Delete(true);
                }
            }
        }
        private SessionVendedor RecuperarVendedor()
        {
            if (Session["userVendedor"] != null)
                return Session["userVendedor"] as SessionVendedor;
            return null;
        }

        #endregion
    }
}