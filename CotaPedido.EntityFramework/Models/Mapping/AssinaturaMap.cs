using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class AssinaturaMap : EntityTypeConfiguration<Assinatura>
    {
        public AssinaturaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ChavePagSeguro)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Assinaturas", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ChavePagSeguro).HasColumnName("ChavePagSeguro");
            this.Property(t => t.IdVendedor).HasColumnName("IdVendedor");
            this.Property(t => t.IdMensalidade).HasColumnName("IdMensalidade");
            this.Property(t => t.DataFinal).HasColumnName("DataFinal");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.FormaPagamento).HasColumnName("FormaPagamento");
        }
    }
}
