using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Cotacao
    {
        public int Id { get; set; }
        public Nullable<int> FormaPagamento { get; set; }
        public Nullable<System.DateTime> DataCadastro { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> IdVendedor { get; set; }
        public Nullable<int> IdPedido { get; set; }
        public string Observacao { get; set; }
        public bool CotacaoEscolhida { get; set; }
        public List<ItemCotacao> Itens { get; set; }
        public Pedido Pedido { get; set; }
        public int SituacaoPedido { get; set; }
    }
}
