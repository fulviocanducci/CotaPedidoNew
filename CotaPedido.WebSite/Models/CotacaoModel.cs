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
    public class CotacaoModel
    {
        #region Construtores

        public CotacaoModel()
        {
            Itens = new List<ItemCotacaoModel>();
            Cotacoes = new List<Cotacao>();
        }

        #endregion

        #region Propriedades Públicas

        public List<ItemCotacaoModel> Itens { get; set; }

        public int Id { get; set; }

        public SituacaoPedido SituacaoPedido { get; set; }

        public int Idvendedor { get; set; }

        [Required(ErrorMessage = "O campo Data Cotação é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Pedido. Ex.: 01/01/2015")]
        public string DataCotacao { get; set; }

        [Required(ErrorMessage = "O campo Data Limite Proposta é obrigatório")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$", ErrorMessage = "Digite uma data válida para Data Limite Proposta. Ex.: 01/01/2015")]
        public string DataLimiteProposta { get; set; }

        [Required(ErrorMessage = "O campo Categoria Principal é obrigatório")]
        public int? IdCategoriaPrincipal { get; set; }

        public Vendedor Vendedor { get { var repositorio = new RepositorioVendedor(); return repositorio.Get(Idvendedor); } }

        public List<Cotacao> Cotacoes { get; set; }

        public List<Pedido> Pedidos { get; set; }
        public Pedido Pedido { get; set; }

        #endregion

        #region Métodos Públicos

        public void CarregarPedidos(int idVendedor)
        {
            var repositorioPedidos = new RepositorioPedido();
            var repositorioGrupos = new RepositorioGrupo();
            
            Pedidos = repositorioPedidos.GetAll().ToList();

            var categoriasIds = Pedidos.Select(x => x.IdCategoriaPrincipal).Distinct().ToList();
            var categorias = repositorioGrupos.GetAll().Where(x => categoriasIds.Contains(x.Id));

            Pedidos.ForEach(x =>
            {
                x.Categoria = categorias.Where(c => c.Id == x.IdCategoriaPrincipal).SingleOrDefault();
            });
        }

        public void CarregarPedido(int idPedido)
        {
            var repositorioPedidos = new RepositorioPedido();
            var repositorioGrupos = new RepositorioGrupo();

            Pedidos = repositorioPedidos.GetAll().Where(p => p.Id == idPedido).ToList();

            var categoriasIds = Pedidos.Select(x => x.IdCategoriaPrincipal).Distinct().ToList();
            var categorias = repositorioGrupos.GetAll().Where(x => categoriasIds.Contains(x.Id));

            Pedidos.ForEach(x =>
            {
                x.Categoria = categorias.Where(c => c.Id == x.IdCategoriaPrincipal).SingleOrDefault();
            });

            Pedido = Pedidos.FirstOrDefault();
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

        public List<SelectListItem> GetSelectCategorias()
        {
            return GetCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome }).ToList();
        }

        public List<SelectListItem> GetSelectSubCategorias()
        {
            return GetSubCategorias().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome }).ToList();
        }

        public void CarregarCotacao(int idCotacao)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var cotacao = repositorioCotacoes.Get(idCotacao);


            //Id = pedido.Id;
            //IdComprador = pedido.IdComprador.Value;
            //DataLimiteProposta = pedido.Validade.Value.ToString("dd/MM/yyyy");
            //DataCotacao = pedido.DataCadastro.Value.ToString("dd/MM/yyyy");
            //IdCategoriaPrincipal = pedido.IdCategoriaPrincipal.Value;
            //SituacaoPedido = pedido.SituacaoPedido;
        }

        public void CarregarItens(int idCotacao)
        {
            var repositorioCotacoes = new RepositorioCotacao();
            var repositorioItens = new RepositorioItemCotacao();
            var repositorioCategorias =    new RepositorioGrupo();
            var repositorioSubCategorias = new RepositorioSubGrupo();

            var pedido = repositorioItens.Get(idCotacao);
            Itens = repositorioItens.GetAll().Where(x => x.IdCotacao == idCotacao).Select(i => new ItemCotacaoModel
            {
                Id = i.Id,
                ValorUnitario= i.ValorUnitario.Value,
                //IdCategoria = i.IdCategoria,
                //IdSubCategoria = i.IdSubCategoria,
                //NomeProduto = i.NomeProduto,
                Quantidade = i.Quantidade.HasValue ? i.Quantidade.Value : 0.0M
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