using System;
using System.Collections.Generic;

namespace CotaPedido.Entidades
{
    public class SubGrupo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Numero { get; set; }
        public int IdGrupo { get; set; }
        public string OrdemExibicao { get; set; }
        public string UrlAmigavel { get; set; }
    }
}
