using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class AnexoPedido
    {
        public int Id { get; set; }
        public byte[] Arquivo { get; set; }
        public int IdPedido { get; set; }
    }
}
