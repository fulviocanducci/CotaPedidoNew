using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class UnidadeMap : EntityTypeConfiguration<Unidade>
    {
        public UnidadeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Descricao)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Unidades", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
        }
    }
}
