using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class RegiaoMap : EntityTypeConfiguration<Regiao>
    {
        public RegiaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(80);

            this.Property(t => t.CodigoArea)
                .HasMaxLength(3);

            // Table & Column Mappings
            this.ToTable("Regioes", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.CodigoArea).HasColumnName("CodigoArea");
            this.Property(t => t.IdUF).HasColumnName("IdUF");
        }
    }
}
