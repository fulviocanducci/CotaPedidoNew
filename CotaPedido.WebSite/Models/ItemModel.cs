using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CotaPedido.WebSite.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public int IdSubcategoria { get; set; }
        public int IdCategoria { get; set; }
        public int IdUnidade { get; set; }
        public decimal Quantidade { get; set; }
        public Grupo Categoria { get { var repositorio = new RepositorioGrupo(); return repositorio.Get(IdCategoria); }}
        public SubGrupo SubCategoria { get { var repositorio = new RepositorioSubGrupo(); return repositorio.Get(IdSubcategoria); } }

      public Unidade GetUnidadeNome()
      {
         var repositorio = new RepositorioUnidade();
         //return repositorio.Get( .Get(IdUnidade);
         return null;
      }
   }
}