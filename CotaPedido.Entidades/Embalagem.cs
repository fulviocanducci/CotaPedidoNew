using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Embalagem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> DataCadastro { get; set; }
    }
}
