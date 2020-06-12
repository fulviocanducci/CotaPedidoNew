using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios;
using System;

namespace CotaPedido.WebSite.Models
{
    public class ItemCotacaoModel
    {
        public int Id { get; set; }
        public int IdCotacao { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public string NomeProduto { get; set; }
        public int IdUnidade { get; set; }
        public int IdItem { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Embalagem { get; set; }
        public int IdSubCategoria { get; set; }
        public int IdCategoria { get; set; }        
        public Grupo Categoria { get { var repositorio = new RepositorioGrupo(); return repositorio.Get(IdCategoria); } }
        public SubGrupo SubCategoria { get { var repositorio = new RepositorioSubGrupo(); return repositorio.Get(IdSubCategoria); } }
    }
}