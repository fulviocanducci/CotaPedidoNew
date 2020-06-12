using System;
namespace CotaPedido.Entidades.Filter
{
    public class PedidoFilter
    {
        public string Ids { get; set; }
        public int? CodigoArea { get; set; }

        public int? IdCidade { get; set; }

        public int? IdComprador { get; set; }

        public int? IdGrupo { get; set; }

        public int? IdSubGrupo { get; set; }

        public int? SituacaoPedido { get; set; }

        public int? Status { get; set; }

        public string DescricaoCategoria { get; set; }
        public string Descricao { get; set; }

        public DateTime? DataLimiteProposta { get; set; }

        public string NomeCidade { get; set; }
    }
}
