using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioGrupo : IRepository<Grupo>
    {
        Grupo Get(string urlAmigavel);
    }
}
