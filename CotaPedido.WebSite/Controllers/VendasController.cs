using CotaPedido.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CotaPedido.WebSite.Models
{
    public class VendasController : Controller
    {
        //
        // GET: /Vendas/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListarCotacoes()
        {
            return View();
        }

      

        public ActionResult ConsultarPedidos()
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            var model = new CotacaoModel();


            model.CarregarPedidos(vendedor.Id);

            return View(model);
        }

        public ActionResult EditarCotacao()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DetalhesPedido(int? id)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            var model = new CotacaoModel();


            model.CarregarPedido(id.Value);

            return View(model);
        }

        #region Métodos Privados

        private Vendedor RecuperarVendedor()
        {
            return Session["userVendedor"] as Vendedor;
        }
        
        [HttpPost]
        public ActionResult Cotar(int? id, CotacaoModel model)
        {
            var vendedor = RecuperarVendedor();
            if (vendedor == null)
            {
                return RedirectToAction("Vendedor", "Cadastro", null);
            }

            model.CarregarPedido(id.Value);

            model.Itens = CarregaItens(model.Pedido.Itens);

            model.DataLimiteProposta = model.Pedido.Validade.Value.ToString("dd/MM/yyyy");
            model.IdCategoriaPrincipal = model.Pedido.IdCategoriaPrincipal.Value;
            model.Idvendedor = vendedor.Id;    
            model.SituacaoPedido = model.Pedido.SituacaoPedido;
            model.DataCotacao = DateTime.Now.ToString("dd/MM/yyyy");

            return View(model);
        }

        #endregion

        private List<ItemCotacaoModel> CarregaItens(List<ItemPedido> list)
        {
            var itens = new List<ItemCotacaoModel>();

            foreach (var item in list)
            {
                itens.Add(new ItemCotacaoModel
                {
                    NomeProduto = item.Descricao,
                    IdItem = item.Id,
                    IdCategoria = item.IdCategoria,
                    IdSubCategoria = item.IdSubCategoria,
                    IdUnidade = 0, //item.IdUnidade.Value,
                    Quantidade = item.Quantidade.Value,
                    ValorUnitario = 0,
                    ValorTotal = 0,
                });
            }

            return itens;
        }
	}
}