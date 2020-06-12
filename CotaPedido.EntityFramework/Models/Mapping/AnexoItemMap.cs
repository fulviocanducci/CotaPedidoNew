using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class AnexoItemMap : EntityTypeConfiguration<AnexoItem>
    {
        public AnexoItemMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.IdItem });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IdItem)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("AnexosItens", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Arquivo).HasColumnName("Arquivo");
            this.Property(t => t.IdItem).HasColumnName("IdItem");
        }
    }
}
