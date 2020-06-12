using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class ItemCotacaoMap : EntityTypeConfiguration<ItemCotacao>
    {
        public ItemCotacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Observacao)
                .HasMaxLength(220);

            this.Property(t => t.Embalagem)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("ItensCotacoes", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.ValorTotal).HasColumnName("ValorTotal");
            this.Property(t => t.Observacao).HasColumnName("Observacao");
            this.Property(t => t.IdUnidade).HasColumnName("IdUnidade");
            this.Property(t => t.IdItem).HasColumnName("IdItem");
            this.Property(t => t.IdCotacao).HasColumnName("IdCotacao");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.Embalagem).HasColumnName("Embalagem");
        }
    }
}
