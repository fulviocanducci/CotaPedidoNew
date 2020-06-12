using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CotaPedido.Entidades;

namespace CotaPedido.EntityFramework.Models.Mapping
{
    public class VendedorMap : EntityTypeConfiguration<Vendedor>
    {
        public VendedorMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.RazaoSocial)
                .HasMaxLength(80);

            this.Property(t => t.NomeFantasia)
                .HasMaxLength(80);

            this.Property(t => t.CNPJ)
                .HasMaxLength(18);

            this.Property(t => t.RG)
                .HasMaxLength(20);

            this.Property(t => t.Endereco)
                .HasMaxLength(120);

            this.Property(t => t.CEP)
                .HasMaxLength(10);

            this.Property(t => t.Telefone)
                .HasMaxLength(14);

            this.Property(t => t.Celular)
                .HasMaxLength(15);

            this.Property(t => t.Numero)
                .HasMaxLength(6);

            this.Property(t => t.Bairro)
                .HasMaxLength(80);

            this.Property(t => t.Complemento)
                .HasMaxLength(80);

            this.Property(t => t.Email)
                .HasMaxLength(60);

            this.Property(t => t.Site)
                .HasMaxLength(100);

            this.Property(t => t.Senha)
                .HasMaxLength(8);

            this.Property(t => t.IdCidade)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IM)
                .HasMaxLength(20);

            this.Property(t => t.IE)
                .HasMaxLength(20);

            this.Property(t => t.EnderecoCobranca)
                .HasMaxLength(120);

            this.Property(t => t.CEPCobranca)
                .HasMaxLength(10);

            this.Property(t => t.TelefoneCobranca)
                .HasMaxLength(14);

            this.Property(t => t.CelularCobranca)
                .HasMaxLength(15);

            this.Property(t => t.NumeroCobranca)
                .HasMaxLength(6);

            this.Property(t => t.BairroCobranca)
                .HasMaxLength(80);

            this.Property(t => t.ComplementoCobranca)
                .HasMaxLength(80);

            this.Property(t => t.IdCidadeCobranca)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Vendedores", "cotapedido");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RazaoSocial).HasColumnName("RazaoSocial");
            this.Property(t => t.NomeFantasia).HasColumnName("NomeFantasia");
            this.Property(t => t.CNPJ).HasColumnName("CNPJ");
            this.Property(t => t.RG).HasColumnName("RG");
            this.Property(t => t.Endereco).HasColumnName("Endereco");
            this.Property(t => t.CEP).HasColumnName("CEP");
            this.Property(t => t.Telefone).HasColumnName("Telefone");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.Bairro).HasColumnName("Bairro");
            this.Property(t => t.Complemento).HasColumnName("Complemento");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Site).HasColumnName("Site");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IdCidade).HasColumnName("IdCidade");
            this.Property(t => t.IE).HasColumnName("IE");
            this.Property(t => t.IM).HasColumnName("IM");
            this.Property(t => t.EnderecoCobranca).HasColumnName("EnderecoCobranca");
            this.Property(t => t.CEPCobranca).HasColumnName("CEPCobranca");
            this.Property(t => t.TelefoneCobranca).HasColumnName("TelefoneCobranca");
            this.Property(t => t.CelularCobranca).HasColumnName("CelularCobranca");
            this.Property(t => t.NumeroCobranca).HasColumnName("NumeroCobranca");
            this.Property(t => t.BairroCobranca).HasColumnName("BairroCobranca");
            this.Property(t => t.ComplementoCobranca).HasColumnName("ComplementoCobranca");
            this.Property(t => t.IdCidadeCobranca).HasColumnName("IdCidadeCobranca");
        }
    }
}
