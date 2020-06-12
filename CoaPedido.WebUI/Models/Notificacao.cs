using CotaPedido.Entidades;

namespace CoaPedido.WebUI.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public int Categoria { get; set; }
        public int? SubCategoria { get; set; }
        public int AreadeAtuacao { get; set; }
        public int Valor { get; set; }
        public int VendedorId { get; set; }

        public static explicit operator Aviso(Notificacao notificacao)
        {
            return new Aviso()
            {
                AvisoId = notificacao.Id,
                AvisoGrupo = notificacao.Categoria,
                AvisoSubGrupo = notificacao.SubCategoria,
                AvisoTipo = notificacao.AreadeAtuacao,
                AvisoPais = notificacao.AreadeAtuacao == 1 ? (int?)notificacao.Valor : null,
                AvisoEstado = notificacao.AreadeAtuacao == 2 ? (int?)notificacao.Valor : null,
                AvisoRegiao = notificacao.AreadeAtuacao == 3 ? (int?)notificacao.Valor : null,
                AvisoCidade = notificacao.AreadeAtuacao == 4 ? (int?)notificacao.Valor : null,
                VendedorId = notificacao.VendedorId
                
            };
        }
    }
}