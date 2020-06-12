using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;
using System.Collections;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioCidade : IRepository<Cidade>
    {
        IEnumerable GetAllWithUf();
    }
}
