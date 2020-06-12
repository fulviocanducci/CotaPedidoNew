using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Mensalidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Nullable<int> QuantidadeDias { get; set; }
        public Nullable<System.DateTime> DataCadastro { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<decimal> Valor { get; set; }
    }
}
