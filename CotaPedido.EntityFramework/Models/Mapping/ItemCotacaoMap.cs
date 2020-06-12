using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class GrupoMap : EntityTypeConfiguration<Grupo>
    {
        public GrupoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Numero)
                .HasMaxLength(10);

            this.Property(t => t.OrdemExibicao)
                .HasMaxLength(2);

            this.Property(t => t.Nome)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Grupos", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.OrdemExibicao).HasColumnName("OrdemExibicao");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
