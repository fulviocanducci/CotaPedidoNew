namespace CotaPedido.Entidades
{
    public class SessionVendedor
    {
        public SessionVendedor(int vendedorId, bool isAssinante, System.DateTime dataCadastro, string email, string nomeFantasia)
        {
            VendedorId = vendedorId;
            IsAssinante = isAssinante;
            DataCadastro = dataCadastro;
            Email = email;
            NomeFantasia = nomeFantasia;
        }

        public int VendedorId { get; }
        public bool IsAssinante { get; }
        public System.DateTime DataCadastro { get; }
        public string Email { get; }
        public string NomeFantasia { get;}

        public static SessionVendedor Create(int vendedorId, bool isAssinante, System.DateTime dataCadastro, string email, string nomeFantasia)
        {
            return new SessionVendedor(vendedorId, isAssinante, dataCadastro, email, nomeFantasia);
        }
    }
}
