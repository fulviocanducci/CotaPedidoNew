using System.Collections.Generic;
using System.Linq;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System.Data.Entity;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioPedido : EFRepositoryBase<Pedido, COTAPEDIDOContext>, IRepositorioPedido
    {
        public RepositorioPedido()
            : base(new COTAPEDIDOContext())
        {}
    }
}
