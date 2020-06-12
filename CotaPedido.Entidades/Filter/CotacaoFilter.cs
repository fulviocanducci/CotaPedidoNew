using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotaPedido.Entidades.Filter
{
    public class CotacaoFilter
    {
        public CotacaoFilter()
        {
            this.NaoCotado = true;
        }

        public int? CodigoArea { get; set; }

        public int? IdCidade { get; set; }

        public int? IdGrupo { get; set; }

        public int? IdSubGrupo { get; set; }

        public int? IdVendedor { get; set; }

        public int? Status { get; set; }

        public int? SituacaoPedido { get; set; }

        public string DescricaoCategoria { get; set; }

        public DateTime? DataLimiteProposta { get; set; }

        public string NomeCidade { get; set; }

        public bool NaoCotado { get; set; }
    }
}
