using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public string OrdemExibicao { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public string UrlAmigavel { get; set; }
    }
}
