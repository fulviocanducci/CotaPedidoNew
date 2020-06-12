using CotaPedido.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CotaPedido.Entidades
{
    public class Pedido
    {
        public Pedido()
        {
            Cotacoes = new List<Cotacao>();
            Itens = new List<ItemPedido>();
        }

        public int Id { get; set; }
        public Nullable<System.DateTime> Validade { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> DataCadastro { get; set; }
        public string NomeCidade { get; set; }
        public Nullable<int> IdComprador { get; set; }
        public string Observacao { get; set; }
        public Nullable<int> IdCategoriaPrincipal { get; set; }
        public SituacaoPedido SituacaoPedido { get; set; }
        public Nullable<int> NumeroCotacoes { get; set; }
        public Grupo Categoria { get; set; }
        public List<ItemPedido> Itens { get; set; }
        public List<Cotacao> Cotacoes { get; set; }
    }
}
