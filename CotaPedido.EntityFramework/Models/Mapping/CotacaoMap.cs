using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class CotacaoMap : EntityTypeConfiguration<Cotacao>
    {
        public CotacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Observacao)
                .HasMaxLength(210);

            // Table & Column Mappings
            this.ToTable("Cotacoes", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FormaPagamento).HasColumnName("FormaPagamento");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.ValorTotal).HasColumnName("ValorTotal");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IdVendedor).HasColumnName("IdVendedor");
            this.Property(t => t.IdPedido).HasColumnName("IdPedido");
            this.Property(t => t.Observacao).HasColumnName("Observacao");
        }
    }
}
