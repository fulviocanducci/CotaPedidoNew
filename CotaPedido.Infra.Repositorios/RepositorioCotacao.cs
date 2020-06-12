using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioCotacao : EFRepositoryBase<Cotacao, COTAPEDIDOContext>, IRepositorioCotacao 
    {
        public RepositorioCotacao()
            : base(new COTAPEDIDOContext())
        {}
    }
}
