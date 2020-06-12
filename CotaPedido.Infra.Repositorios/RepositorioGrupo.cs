using CotaPedido.Entidades;
using CotaPedido.EntityFramework.Models;
using CotaPedido.Infra.IRepositorios;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioGrupo : EFRepositoryBase<Grupo, COTAPEDIDOContext>, IRepositorioGrupo
    {
        public RepositorioGrupo()
            : base(new COTAPEDIDOContext())
        {}

        public Grupo Get(string urlAmigavel)
        {
            throw new System.NotImplementedException();
        }
    }
}
