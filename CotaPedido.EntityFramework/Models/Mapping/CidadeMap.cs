using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class CidadeMap : EntityTypeConfiguration<Cidade>
    {
        public CidadeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Nome)
                .HasMaxLength(80);

            this.Property(t => t.CodIbge)
                .HasMaxLength(8);

            this.Property(t => t.IdRegiao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Cidades", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.CodIbge).HasColumnName("CodIbge");
            this.Property(t => t.IdRegiao).HasColumnName("IdRegiao");
            this.Property(t => t.UF).HasColumnName("UF");
            this.Property(t => t.IdPais).HasColumnName("IdPais");
        }
    }
}
