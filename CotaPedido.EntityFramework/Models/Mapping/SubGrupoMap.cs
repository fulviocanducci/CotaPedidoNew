using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class SubGrupoMap : EntityTypeConfiguration<SubGrupo>
    {
        public SubGrupoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Nome)
                .HasMaxLength(80);

            this.Property(t => t.Numero)
                .HasMaxLength(10);

            this.Property(t => t.IdGrupo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OrdemExibicao)
                .HasMaxLength(3);

            // Table & Column Mappings
            this.ToTable("SubGrupos", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.IdGrupo).HasColumnName("IdGrupo");
            this.Property(t => t.OrdemExibicao).HasColumnName("OrdemExibicao");
        }
    }
}
