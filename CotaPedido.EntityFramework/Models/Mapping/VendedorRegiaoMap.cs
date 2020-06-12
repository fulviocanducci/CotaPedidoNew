using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class VendedorRegiaoMap : EntityTypeConfiguration<VendedorRegiao>
    {
        public VendedorRegiaoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdVendedor, t.IdRegiao });

            // Properties
            this.Property(t => t.IdVendedor)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IdRegiao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VendedoresRegioes", "cotapedido");
            this.Property(t => t.IdVendedor).HasColumnName("IdVendedor");
            this.Property(t => t.IdRegiao).HasColumnName("IdRegiao");
        }
    }
}
