using CotaPedido.Entidades;
using CotaPedido.EntityFramework.Models;
using CotaPedido.Infra.IRepositorios;
using System.Collections.Generic;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioItemPedido : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioItemPedido
    {
        public RepositorioItemPedido()
            : base(new COTAPEDIDOContext())
        {}

      public IEnumerable<int> Insert(IEnumerable<ItemPedido> entities)
      {
         throw new System.NotImplementedException();
      }
   }
}
