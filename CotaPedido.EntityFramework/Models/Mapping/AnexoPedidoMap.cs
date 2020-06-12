using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class AnexoPedidoMap : EntityTypeConfiguration<AnexoPedido>
    {
        public AnexoPedidoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.IdPedido });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IdPedido)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("AnexosPedidos", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Arquivo).HasColumnName("Arquivo");
            this.Property(t => t.IdPedido).HasColumnName("IdPedido");
        }
    }
}
