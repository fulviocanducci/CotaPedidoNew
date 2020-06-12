using System;
namespace CotaPedido.Entidades
{
    public class Assinatura
    {
        public int Id { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Status { get; set; }
        public string ChavePagSeguro { get; set; }
        public int? IdVendedor { get; set; }
        public int? IdMensalidade { get; set; }
        public DateTime? DataFinal { get; set; }
        public decimal? Valor { get; set; }
        public int? FormaPagamento { get; set; }
    }
}
