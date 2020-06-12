using CotaPedido.Entidades;
using CotaPedido.Infra.Repositorios.SQLServer;

namespace CotaPedido.WebUI.Models
{
    public class ItemModel
    {
        public int Id { get; set; }

        public string NomeProduto { get; set; }

        public int IdSubcategoria { get; set; }

        public int IdCategoria { get; set; }

        public int IdUnidade { get; set; }

        public decimal Quantidade { get; set; }

        public bool Excluido { get; set; }

        public Grupo Categoria { get; set; }

        public SubGrupo SubCategoria { get; set; }

        public Unidade Unidade { get; set; }
    }
}