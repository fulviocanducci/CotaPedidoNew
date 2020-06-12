namespace CotaPedido.Entidades
{
    public class Aviso
    {
        public int AvisoId { get; set; }
        public int AvisoTipo { get; set; }        
        public int AvisoGrupo { get; set; }
        public int VendedorId { get; set; }
        public int? AvisoSubGrupo { get; set; }
        public int? AvisoPais { get; set; }
        public int? AvisoRegiao { get; set; }
        public int? AvisoEstado { get; set; }
        public int? AvisoCidade { get; set; }       
    }   

    public class AvisoViewModel: Aviso
    {        
        public string AvisoTipoDescription {
            get
            {
                return System.Enum.GetName(typeof(Enum.AvisoTipo), AvisoTipo);
            }
        }
        public string Valor { get; set; }
        public string GrupoNome { get; set; }
        public string SubGrupoNome { get;set;  }
    }

    public class DefaultViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
