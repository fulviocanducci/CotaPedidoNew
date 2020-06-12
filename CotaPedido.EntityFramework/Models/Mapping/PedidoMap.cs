using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class PedidoMap : EntityTypeConfiguration<Pedido>
    {
        public PedidoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Observacao)
                .IsFixedLength()
                .HasMaxLength(18);

            // Table & Column Mappings
            this.ToTable("Pedidos", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Validade).HasColumnName("Validade");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.IdComprador).HasColumnName("IdComprador");
            this.Property(t => t.Observacao).HasColumnName("Observação");
            this.Property(t => t.IdCategoriaPrincipal).HasColumnName("IdGrupoPrincipal");
            this.Property(t => t.SituacaoPedido).HasColumnName("SituacaoPedido");
        }
    }
}
