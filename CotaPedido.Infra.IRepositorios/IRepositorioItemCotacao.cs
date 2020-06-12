using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;
using System.Collections;
using System.Collections.Generic;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioItemCotacao : IRepository<ItemCotacao>
    {
        int UpdateFromExcel(int id, int item, decimal valorUnitario, decimal valorTotal);
        IEnumerable<int> UpdateFromExcel(IEnumerable items);
    }
}
