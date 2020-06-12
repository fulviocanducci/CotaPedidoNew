using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Busca
    {
        public int Id { get; set; }
        public string BuscaTexto { get; set; }
        public Nullable<int> IdVendedor { get; set; }
        public Nullable<int> IdCidade { get; set; }
        public Nullable<int> IdRegiao { get; set; }
    }
}
