using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class EmbalagemMap : EntityTypeConfiguration<Embalagem>
    {
        public EmbalagemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Embalagens", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
        }
    }
}
