using CoaPedido.WebUI.Models;
using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Excel;
using CotaPedido.Infra.Repositorios.SQLServer;
using CotaPedido.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CoaPedido.WebUI.Controllers
{
    [SessionExpire("user")]
    public class ComprasController : Controller
    {
        #region Métodos Públicos

        public ActionResult Index(PedidoModel model, int? page)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            model.IdComprador = comprador.Id;
            model.CarregarPedidos(page);

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_ListarPedidos", model)
                : View(model);
        }

        [HttpGet]
        public ActionResult Solicitar()
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }
            var model = new PedidoModel { IdComprador = comprador.Id, DataPedido = DateTime.Now.ToString("dd/MM/yyyy") };

            return View(model);
        }

        [HttpPost]
        public ActionResult Solicitar(PedidoModel model)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            DateTime data;
            if (DateTime.TryParse(model.DataLimiteProposta, out data))
            {
                if (data < DateTime.Now)
                {
                    ModelState.AddModelError("DataLimiteProposta", "A Data Limite Proposta não pode ser inferior a data atual");
                }
            }

            if (ModelState.IsValid)
            {
                var pedido = new Pedido
                {
                    DataCadastro = Convert.ToDateTime(model.DataPedido),
                    Validade = Convert.ToDateTime(model.DataLimiteProposta),
                    SituacaoPedido = SituacaoPedido.Pendente,
                    IdComprador = model.IdComprador,
                    IdCategoriaPrincipal = model.IdCategoriaPrincipal,
                    Status = true
                };

                var pedidoRepositorio = new RepositorioPedido();
                model.Id = pedidoRepositorio.Insert(pedido);

                TempData.Add("Pedido", model);
                return RedirectToAction("Nova");
            }

            return View(model);
        }

        public ActionResult Nova()
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            var model = TempData["Pedido"] as PedidoModel;
            if (model == null)
            {
                return RedirectToAction("Solicitar");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nova(PedidoModel model)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }
            model.IdComprador = comprador.Id;

            if (model.Itens.Count == 0)
            {
                ModelState.AddModelError("", "Inclua pelo menos um item em seu pedido");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.InserirItensPedido();

                    TempData.Add("PedidoCadastrado", true);

                    return RedirectToAction("Index");
                }
                catch (SmtpException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar gravar pedido");
                }
            }
            return View(model);
        }

        public ActionResult EditarPedido(int id)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            var model = new PedidoModel();
            model.CarregarPedido(id);
            model.CarregarItens(id);
            
            if (TempData.ContainsKey("Status"))
            {
                ViewBag.Status = TempData["Status"];
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPedido(PedidoModel model)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }
            model.IdComprador = comprador.Id;

            if (model.Itens.Count == 0)
            {
                ModelState.AddModelError("", "Inclua pelo menos um item em seu pedido");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.EditarItensPedido();

                    TempData.Add("PedidoAlterado", true);

                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar editar pedido");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Comparar(int id, string message=null)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", new { link = Request.Url.PathAndQuery });
            }

            var model = new PedidoModel();
            model.CarregarPedido(id);
            model.CarregarItens(id);
            model.CarregarCotacoesPedido(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Comparar(int idCotacao, int idPedido)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            var model = new PedidoModel();
            model.CarregarCotacoesPedido(idPedido);
            model.CarregarPedido(idPedido);
            model.CarregarItens(idPedido);
            model.Id = idPedido;

            try
            {
                model.ConfirmarCotacao(idCotacao, comprador);

                TempData.Add("PedidoCadastrado", true);

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Ocorreu um erro ao tentar confirmar cotação");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GetSubCategorias(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                throw new ArgumentException("O parâmetro idCategoria é obrigatório");
            }

            var subCategoriaRepositorio = new RepositorioSubGrupo();
            var subCategorias = subCategoriaRepositorio.GetAll().Where(x => x.IdGrupo == idCategoria);

            return Json(subCategorias);
        }

        [HttpPost]
        public ActionResult AdicionarSubcategoria(string nome, int idCategoria)
        {
            var message = "";
            var select = "";
            var success = false;
            if (string.IsNullOrEmpty(nome))
            {
                message = "O campo Nome é obrigatório.";
            }
            else
            {
                var repositorioSubcategoria = new RepositorioSubGrupo();
                try
                {
                    var subGrupo = new SubGrupo { IdGrupo = idCategoria, Nome = nome };

                    var subGrupos = repositorioSubcategoria.GetAll().Where(x => x.Nome.ToUpper() == subGrupo.Nome.ToUpper()).ToList();
                    if (subGrupos.Count() > 0)
                    {
                        message = "Já existe uma subcategoria com esse nome";
                    }
                    else
                    {
                        subGrupo.Id = repositorioSubcategoria.Insert(subGrupo);
                        subGrupos = repositorioSubcategoria.GetAll().Where(x => x.IdGrupo == idCategoria).ToList();
                        subGrupos.ForEach(x =>
                        {
                            select += string.Format("<option value=\"{0}\"{1}>{2}</option>", x.Id.ToString(), x.Id == subGrupo.Id ? " selected" : "", x.Nome);
                        });
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                }
            }
            return Json(new { Message = message, Success = success, Select = select });
        }

        [HttpPost]
        public ActionResult ImportarItens(HttpPostedFileBase arquivo, int id, FormCollection form)
        {
            ImportFromExcelToComprador importFromExcel = null;
            var comprador = RecuperarComprador();
            try
            {
                if (arquivo.InputStream.Length > 0)
                {
                    importFromExcel = new ImportFromExcelToComprador();

                    arquivo.InputStream.Position = 0;
                    var dados = importFromExcel.Render(arquivo.InputStream).ToList();

                    var categorias = dados.Where(x => !string.IsNullOrEmpty(x.Categoria)).Select(s => s.Categoria).Distinct();
                    var subcategorias = dados.Where(x => !string.IsNullOrEmpty(x.SubCategoria)).Select(s => s.SubCategoria).Distinct();
                    var unidades = dados.Where(x => !string.IsNullOrEmpty(x.Unidade)).Select(s => s.Unidade).Distinct();

                    RepositorioSubGrupo repSubGrupo = new RepositorioSubGrupo();
                    var resultSubGrupo = repSubGrupo.GetList(new { Nomes = subcategorias });

                    RepositorioUnidade repUnidade = new RepositorioUnidade();
                    var resultUnidade = repUnidade.GetList(new { Descricoes = unidades });
                                       
                    dados.ForEach(x =>
                    {
                        x.UnidadeId = resultUnidade
                            .Where(s => s.Descricao == x.Unidade)
                            .Select(a => a.Id)
                            .FirstOrDefault();
                        var item = resultSubGrupo
                            .Where(s => s.Nome == x.SubCategoria)
                            .Select(a => new { a.IdGrupo, SubGrupoId = a.Id })
                            .FirstOrDefault();
                        if (item != null)
                        {
                            x.SubCategoriaId = item.SubGrupoId;
                            x.CategoriaId = item.IdGrupo;
                        }
                    });

                    repSubGrupo = null;
                    repUnidade = null;
                    resultSubGrupo = null;
                    resultUnidade = null;

                    if (dados.Count > 0 && dados.Where(x => x.IsValid).Count() > 0)
                    {
                        IEnumerable<ItemPedido> entities = dados.Where(x => x.IsValid)
                                .Select(item => new ItemPedido
                                {
                                     Descricao = item.Produto,
                                     IdPedido = id,
                                     IdUnidade = item.UnidadeId.Value,
                                     IdCategoria = item.CategoriaId.Value,
                                     IdSubCategoria = item.SubCategoriaId.Value,
                                     Quantidade = item.Quantidade
                                });
                        if (entities.Count() > 0)
                        {
                            RepositorioItemPedido repItemPedido = new RepositorioItemPedido();
                            var result = repItemPedido
                                .Insert(entities)
                                .ToList();
                            repItemPedido = null;
                            if (result.Any(x => x == 0) == false)
                            {
                                TempData["Status"] = "Itens do pedido inseridos";
                            }
                            else
                            {
                                TempData["Status"] = "Não houve alteração dados conflitantes: Itens enviados são inválidos";
                            }
                        }
                        else
                        {
                            TempData["Status"] = "Não houve alteração dados conflitantes: Itens enviados são inválidos";
                        }
                    }
                    else
                    {
                        TempData["Status"] = "Não houve alteração dados conflitantes: Itens enviados são inválidos";
                    }
                    importFromExcel = null;
                }
            }
            catch (Exception)
            {
                TempData["Status"] = "Problemas no processamento, entre em contato com administrador";
            }
            finally
            {
                importFromExcel = null;
            }
            return RedirectToAction("EditarPedido", new { id });
        }

        #region old_ImportarIntes
        //[HttpPost]
        //public ActionResult ImportarItens(int id, HttpPostedFileBase file)
        //{
        //    var comprador = RecuperarComprador();
        //    if (comprador == null)
        //    {
        //        return RedirectToAction("Comprador", "Cadastro", null);
        //    }

        //    try
        //    {
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            var tempFolder = comprador.NomeFantasia;
        //            if (!Directory.Exists(Server.MapPath("~/Temp/" + tempFolder)))
        //                Directory.CreateDirectory(Server.MapPath("~/Temp/" + tempFolder));

        //            //Save file content goes here
        //            var pathString = Path.Combine(Server.MapPath("~/Temp/" + tempFolder), Path.GetFileName(file.FileName));
        //            file.SaveAs(pathString);

        //            var model = new PedidoModel();
        //            model.Id = id;
        //            model.LerArquivoItensPedido(pathString);
        //        }
        //        else
        //        {
        //            return Json(new { result = "Erro", erro = "Selecione um arquivo para importar" });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { result = "Erro", erro = "Erro ao importar arquivo" });
        //    }
        //    finally
        //    {
        //        RemoveAllTempFiles();
        //    }

        //    TempData.Add("PedidoCadastrado", true);

        //    return Json(new { result = "Redirect", url = Url.Action("Index", "Compras") });
        //}
        #endregion

        [HttpPost]
        public ActionResult CancelarPedido(int idPedido)
        {
            try
            {
                var comprador = RecuperarComprador();
                if (comprador == null)
                {
                    return RedirectToAction("Comprador", "Cadastro", null);
                }

                var model = new PedidoModel();
                model.IdComprador = comprador.Id;
                model.CancelarPedido(idPedido);
            }
            catch
            {
                return Json(new { result = "Erro", erro = "Erro ao cancelar pedido!" });
            }

            TempData.Add("PedidoCancelado", true);

            return Json(new { result = "Redirect", url = Url.Action("Index", "Compras") });
        }

        [HttpPost]
        public ActionResult PublicarPedido(int idPedido)
        {
            try
            {
                var comprador = RecuperarComprador();
                if (comprador == null)
                {
                    return RedirectToAction("Comprador", "Cadastro", null);
                }

                var model = new PedidoModel();
                model.IdComprador = comprador.Id;
                model.PublicarPedido(idPedido);
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message, result = "Erro", erro = "Erro ao publicar pedido!" });
            }

            TempData.Add("PedidoPublicado", true);

            return Json(new { result = "Redirect", url = Url.Action("Index", "Compras") });
        }

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

        private Comprador RecuperarComprador()
        {
            return Session["user"] as Comprador;
        }

        #endregion
    }
}