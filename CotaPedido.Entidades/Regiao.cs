using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Regiao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CodigoArea { get; set; }
        public Nullable<int> IdUF { get; set; }
    }
}
