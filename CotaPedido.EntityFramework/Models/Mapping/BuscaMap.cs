using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class BuscaMap : EntityTypeConfiguration<Busca>
    {
        public BuscaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Buscas", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BuscaTexto).HasColumnName("BuscaTexto");
            this.Property(t => t.IdVendedor).HasColumnName("IdVendedor");
            this.Property(t => t.IdCidade).HasColumnName("IdCidade");
            this.Property(t => t.IdRegiao).HasColumnName("IdRegiao");
        }
    }
}
