using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class ItemPedido
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
      
        public Nullable<int> IdEmbalagem { get; set; }
        public Nullable<int> Numero { get; set; }
        public string Descricao { get; set; }
        public Nullable<decimal> ValorUnitario { get; set; }
        public Nullable<decimal> Quantidade { get; set; }
        public string Observacao { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public int IdCategoria { get; set; }
        public int IdSubCategoria { get; set; }
        public int IdUnidade { get; set; }

        public Pedido Pedido { get; set; }
        public Unidade Unidade { get; set; }
        public Grupo Categoria { get; set; }
        public SubGrupo SubCategoria { get; set; }
    }
}
