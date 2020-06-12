using CotaPedido.Entidades;
using CotaPedido.Dominio.Interface;
using System.Collections.Generic;

namespace CotaPedido.Infra.IRepositorios
{
    public interface IRepositorioAssinatura : IRepository<Assinatura>
    {
        IEnumerable<AssinaturaViewModel> GetVendedorId(int id);
        bool GetVendedorIdIsAssinante(int id);

        int AtualizaStatusPagamento(Assinatura entity);

    }
}
