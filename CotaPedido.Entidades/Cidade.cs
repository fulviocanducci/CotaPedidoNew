using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CodIbge { get; set; }
        public Nullable<int> IdRegiao { get; set; }
        public int UF { get; set; }
        public int IdPais { get; set; }
    }
}
