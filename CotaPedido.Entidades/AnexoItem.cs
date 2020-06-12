using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class AnexoItem
    {
        public int Id { get; set; }
        public byte[] Arquivo { get; set; }
        public int IdItem { get; set; }
    }
}
