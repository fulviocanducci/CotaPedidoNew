using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class ItemCotacao
    {
        public int Id { get; set; }
        public Nullable<decimal> ValorUnitario { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public Nullable<decimal> Quantidade { get; set; }
        public string Observacao { get; set; }
        public Nullable<int> IdUnidade { get; set; }
        public Nullable<int> IdItem { get; set; }
        public Nullable<int> IdCotacao { get; set; }
        public Nullable<System.DateTime> DataCadastro { get; set; }
        public string Embalagem { get; set; }
    }
}
