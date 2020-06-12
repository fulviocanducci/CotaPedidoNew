using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class MensalidadeMap : EntityTypeConfiguration<Mensalidade>
    {
        public MensalidadeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Mensalidades", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.QuantidadeDias).HasColumnName("QuantidadeDias");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
