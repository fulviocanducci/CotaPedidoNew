using CotaPedido.Entidades;
using CotaPedido.Entidades.Enum;
using CotaPedido.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CotaPedido.WebSite.Models
{
    public class PedidoModel
    {
        #region Construtores

        public PedidoModel()
        {
            Itens = new List<ItemModel>();
            Pedidos = new List<Pedido>();
        }

        #endregion

        #region Propriedades Públicas

        public List<ItemModel> Itens { get; set; }

        public int Id { get; set; }

        public SituacaoPedido SituacaoPedido { get; set; }
        public string NomeCidade { get; set; }

        public string Observacao { get; set; }

        public int IdComprador { get; set; }

        [Required(ErrorMessage = "O campo Data Pedido é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Pedido. Ex.: 01/01/2015")]
        public string DataPedido { get; set; }

        [Required(ErrorMessage = "O campo Data Limite Proposta é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Limite Proposta. Ex.: 01/01/2015")]
        public string DataLimiteProposta { get; set; }

        [Required(ErrorMessage = "O campo Categoria Principal é obrigatório")]

        public int IdCategoriaPrincipal { get; set; }

        public Comprador Comprador { get { var repositorio = new RepositorioComprador(); return repositorio.Get(IdComprador); } }

        public List<Pedido> Pedidos { get; set; }

        #endregion

        #region Métodos Públicos

        public void CarregarPedidos(int idComprador)
        {
            var repositorioPedidos = new RepositorioPedido();
            var repositorioGrupos = new RepositorioGrupo();
            Pedidos = repositorioPedidos.GetAll().Where(x => x.IdComprador == idComprador).ToList();
            var categoriasIds = Pedidos.Select(x => x.IdCategoriaPrincipal).Distinct().ToList();
            var categorias = repositorioGrupos.GetAll().Where(x => categoriasIds.Contains(x.Id));

            Pedidos.ForEach(x =>
            {
                x.Categoria = categorias.Where(c => c.Id == x.IdCategoriaPrincipal).SingleOrDefault();
            });
        }

        public List<Grupo> GetCategorias()
        {
            var grupoRepositorio = new RepositorioGrupo();
            return grupoRepositorio.GetAll().OrderBy(x => x.OrdemExibicao).ToList();
        }

        public List<SubGrupo> GetSubCategorias()
        {
            var subGrupoRepositorio = new RepositorioSubGrupo();
            return subGrupoRepositorio.GetAll().OrderBy(x => x.OrdemExibicao).ToList();
        }
        public List<Unidade> GetUnidades()
        {
            var grupoUnidade = new RepositorioUnidade();
         //return grupoUnidade.GetAll().OrderBy(x => x.Descricao).ToList();
         return null;
        }

        public List<SelectListItem> GetSelectCategorias()
        {
            return GetCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome }).ToList();
        }

        public List<SelectListItem> GetSelectSubCategorias()
        {
            return GetSubCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome }).ToList();
        }
        public List<SelectListItem> GetSelectUnidades()
        {
            return GetUnidades().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Descricao }).ToList();
        }

        public void CarregarPedido(int idPedido)
        {
            var repositorioPedidos = new RepositorioPedido();
            var pedido = repositorioPedidos.Get(idPedido);
            Id = pedido.Id;
            IdComprador = pedido.IdComprador.Value;
            DataLimiteProposta = pedido.Validade.Value.ToString("dd/MM/yyyy");
            DataPedido = pedido.DataCadastro.Value.ToString("dd/MM/yyyy");
            IdCategoriaPrincipal = pedido.IdCategoriaPrincipal.Value;
            SituacaoPedido = pedido.SituacaoPedido;
            Observacao = pedido.Observacao;
            NomeCidade = pedido.NomeCidade;
        }

        public void CarregarItens(int idPedido)
        {
            var repositorioPedidos = new RepositorioPedido();
            var repositorioItens = new RepositorioItemPedido();
            var repositorioCategorias =    new RepositorioGrupo();
            var repositorioSubCategorias = new RepositorioSubGrupo();
            var repositorioUnidades = new RepositorioUnidade();
            var pedido = repositorioPedidos.Get(idPedido);
            Itens = repositorioItens.GetAll().Where(x => x.IdPedido == idPedido).Select(i => new ItemModel
            {
                Id = i.Id,
                IdCategoria = i.IdCategoria,
                IdSubcategoria = i.IdSubCategoria,
                NomeProduto = i.Descricao,
                Quantidade = i.Quantidade.HasValue ? i.Quantidade.Value : 0.0M,
                IdUnidade = i.IdUnidade
                
            }).ToList();

            //var idsCategorias = Itens.Select(x => x.IdCategoria).Distinct();
            //var idsSubCategorias = Itens.Select(x => x.IdSubcategoria).Distinct();

            //var categorias = repositorioCategorias.GetAll().Where(x => idsCategorias.Contains(x.Id));
            //var subcategorias = repositorioSubCategorias.GetAll().Where(x => idsSubCategorias.Contains(x.Id));

            //Itens.ForEach(x =>
            //{
            //    x.Categoria = categorias.Where(c => c.Id == x.IdCategoria).ToList().SingleOrDefault();
            //    x.SubCategoria = subcategorias.Where(c => c.Id == x.IdSubcategoria).ToList().SingleOrDefault();
            //});
        }

        #endregion
    }
}