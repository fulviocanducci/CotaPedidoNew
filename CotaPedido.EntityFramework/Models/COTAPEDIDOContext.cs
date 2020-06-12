using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CotaPedido.Entidades;
using CotaPedido.EntityFramework.Models.Mapping;

namespace CotaPedido.EntityFramework.Models
{
    public partial class COTAPEDIDOContext : DbContext
    {
        static COTAPEDIDOContext()
        {
            Database.SetInitializer<COTAPEDIDOContext>(null);
        }

        public COTAPEDIDOContext()
            : base("Name=COTAPEDIDOContext")
        {
        }

        public DbSet<AnexoItem> AnexosItens { get; set; }
        public DbSet<AnexoPedido> AnexosPedidos { get; set; }
        public DbSet<Assinatura> Assinaturas { get; set; }
        public DbSet<Busca> Buscas { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Comprador> Compradores { get; set; }
        public DbSet<Cotacao> Cotacoes { get; set; }
        public DbSet<Embalagem> Embalagens { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<ItemCotacao> ItensCotacoes { get; set; }
        public DbSet<ItemPedido> ItensPedidos { get; set; }
        public DbSet<Mensalidade> Mensalidades { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<SubGrupo> SubGrupos { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<VendedorRegiao> VendedoresRegioes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AnexoItemMap());
            modelBuilder.Configurations.Add(new AnexoPedidoMap());
            modelBuilder.Configurations.Add(new AssinaturaMap());
            modelBuilder.Configurations.Add(new BuscaMap());
            modelBuilder.Configurations.Add(new CidadeMap());
            modelBuilder.Configurations.Add(new CompradorMap());
            modelBuilder.Configurations.Add(new CotacaoMap());
            modelBuilder.Configurations.Add(new EmbalagemMap());
            modelBuilder.Configurations.Add(new GrupoMap());
            modelBuilder.Configurations.Add(new ItemCotacaoMap());
            modelBuilder.Configurations.Add(new ItemPedidoMap());
            modelBuilder.Configurations.Add(new MensalidadeMap());
            modelBuilder.Configurations.Add(new PedidoMap());
            modelBuilder.Configurations.Add(new RegiaoMap());
            modelBuilder.Configurations.Add(new SubGrupoMap());
            modelBuilder.Configurations.Add(new UnidadeMap());
            modelBuilder.Configurations.Add(new VendedorMap());
            modelBuilder.Configurations.Add(new VendedorRegiaoMap());


            modelBuilder.Properties()
                .Where(p => p.Name == "Id")
                .Configure(p => p.IsKey());

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnName("varchar"));

            modelBuilder.Properties<string>()
                .Configure(p => p.HasMaxLength(100));
        }
    }
}
