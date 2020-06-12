using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;
using System.Collections.Generic;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioItemPedido : IRepository<ItemPedido>
    {
        IEnumerable<int> Insert(IEnumerable<ItemPedido> entities);
    }
}
