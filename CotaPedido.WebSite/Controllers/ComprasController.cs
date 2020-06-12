using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Infra.Repositorios;
using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace CotaPedido.WebSite.Models
{
    public class ComprasController : Controller
    {
        #region Métodos Públicos

        public ActionResult Index()
        {
            return View();
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
                    SituacaoPedido = SituacaoPedido.EmAberto,
                    IdComprador = model.IdComprador,
                    IdCategoriaPrincipal = model.IdCategoriaPrincipal,
                    Status = true
                };

                var pedidoRepositorio = new RepositorioPedido();
                pedidoRepositorio.Insert(pedido);
                model.Id = pedido.Id;

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

            if (model.Itens.Count == 0)
            {
                ModelState.AddModelError("", "Inclua pelo menos um item em seu pedido");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var itensPedido = model.Itens.Select(x => new ItemPedido
                    {
                        IdPedido = model.Id,
                        Quantidade = x.Quantidade,
                        Descricao = x.NomeProduto,
                        IdCategoria = x.IdCategoria,
                        IdSubCategoria = x.IdSubcategoria,
                        IdUnidade = x.IdUnidade
                    }).ToList();

                    using (var scope = new TransactionScope())
                    {
                        var pedidoRepositorio = new RepositorioPedido();
                        var pedido = pedidoRepositorio.Get(model.Id);
                        pedido.SituacaoPedido = SituacaoPedido.Pendente;
                        pedidoRepositorio.Update(pedido);

                        var itemPedidoRepositorio = new RepositorioItemPedido();

                        foreach (var item in itensPedido)
                        {
                            itemPedidoRepositorio.Insert(item);
                        }

                        scope.Complete();
                    }
                    TempData.Add("PedidoCadastrado", true);
                    return RedirectToAction("Solicitar");
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar gravar pedido");
                }
            }
            return View(model);
        }

        public ActionResult Editar()
        {
            return View();
        }

        public ActionResult Listar()
        {
            return View();
        }

        public ActionResult ListarPedidos()
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            if (TempData["PedidoEditado"] != null)
            {
                ViewBag.EditSuccess = true;
            }

            var model = new PedidoModel();
            model.CarregarPedidos(comprador.Id);

            return View(model);
        }

        public ActionResult EditarPedido(int Id)
        {
            var comprador = RecuperarComprador();
            if (comprador == null)
            {
                return RedirectToAction("Comprador", "Cadastro", null);
            }

            var model = new PedidoModel();
            model.CarregarItens(Id);
            model.CarregarPedido(Id);
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

            if (model.Itens.Count == 0)
            {
                ModelState.AddModelError("", "Inclua pelo menos um item em seu pedido");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var itensPedido = model.Itens.Select(x => new ItemPedido
                    {
                        Id = x.Id,
                        IdPedido = model.Id,
                        Quantidade = x.Quantidade,
                        Descricao = x.NomeProduto,
                        IdCategoria = x.IdCategoria,
                        IdSubCategoria = x.IdSubcategoria
                    }).ToList();

                    var itemPedidoRepositorio = new RepositorioItemPedido();

                    var itensPedidoEditar = itensPedido.Where(x => x.Id != 0).ToList();
                    var itensPedidoInserir = itensPedido.Where(x => x.Id == 0).ToList();

                    using (var scope = new TransactionScope())
                    {
                        var pedidoRepositorio = new RepositorioPedido();
                        var pedido = pedidoRepositorio.Get(model.Id);
                        pedido.SituacaoPedido = SituacaoPedido.Pendente;
                        pedidoRepositorio.Update(pedido);

                        foreach (var item in itensPedidoInserir)
                        {
                            itemPedidoRepositorio.Insert(item);
                        }

                        foreach (var item in itensPedidoEditar)
                        {
                            itemPedidoRepositorio.Update(item);
                        }
                        
                        scope.Complete();
                    }

                    TempData.Add("PedidoEditado", true);
                    return RedirectToAction("ListarPedidos");
                }
                catch
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao tentar editar pedido");
                }
            }

            return View(model);
        }

        public ActionResult Comparar()
        {
            return View();
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
                var repositorioCategoria = new RepositorioGrupo();
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
                        success = true;
                        repositorioSubcategoria.Insert(subGrupo);
                        subGrupos = repositorioSubcategoria.GetAll().Where(x => x.IdGrupo == idCategoria).ToList();
                        subGrupos.ForEach(x =>
                        {
                            select += string.Format("<option value=\"{0}\"{1}>{2}</option>", x.Id.ToString(), x.Id == subGrupo.Id ? " selected" : "", x.Nome);
                        });
                    }
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                }
            }
            return Json(new { Message = message, Success = success, Select = select });
        }
        #endregion

        #region Métodos Privados

        private Comprador RecuperarComprador()
        {
            return Session["user"] as Comprador;
        }

        #endregion
    }
}