using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class ItemPedidoMap : EntityTypeConfiguration<ItemPedido>
    {
        public ItemPedidoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.IdPedido });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.IdPedido)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IdPedido)
                .IsRequired();

            this.Property(t => t.IdUnidade)
                .IsRequired();

            this.Property(t => t.IdSubCategoria)
                .IsRequired();

            this.Property(t => t.Descricao)
                .HasMaxLength(120);

            // Table & Column Mappings
            this.ToTable("ItensPedidos", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IdPedido).HasColumnName("IdPedido");
            this.Property(t => t.IdUnidade).HasColumnName("IdUnidade");
            this.Property(t => t.IdEmbalagem).HasColumnName("IdEmbalagem");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.ValorUnitario).HasColumnName("ValorUnitario");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade");
            this.Property(t => t.Observacao).HasColumnName("Observacao");
            this.Property(t => t.ValorTotal).HasColumnName("ValorTotal");
            this.Property(t => t.IdCategoria).HasColumnName("IdGrupo");
            this.Property(t => t.IdSubCategoria).HasColumnName("IdSubGrupo");

            this.HasRequired(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.IdPedido);
        }
    }
}
