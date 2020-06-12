using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioComprador : IRepository<Comprador>
    {        
        bool GetEmailExists(string email);
        int Insert(
            string email,
            string senha,
            string nomeFantasia,
            string razaoSocial,
            int idCidade,
            int idCidadeCobranca,
            System.DateTime dataCadastro,
            bool status = true);
    }
}
