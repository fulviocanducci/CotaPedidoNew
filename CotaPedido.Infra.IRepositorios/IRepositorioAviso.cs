using CotaPedido.Dominio.Interface;
using CotaPedido.Entidades;
using System.Collections.Generic;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioAviso: IRepository<Aviso>
    {
        IList<AvisoViewModel> GetAvisosFromVendedor(int? id);
    }
}
