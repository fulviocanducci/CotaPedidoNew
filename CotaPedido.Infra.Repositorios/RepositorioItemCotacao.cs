using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System.Collections.Generic;
using System.Collections;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioItemCotacao : EFRepositoryBase<ItemCotacao, COTAPEDIDOContext>, IRepositorioItemCotacao 
    {
        public RepositorioItemCotacao()
            : base(new COTAPEDIDOContext())
        {}

      public int UpdateFromExcel(int id, int item, decimal valorUnitario, decimal valorTotal)
      {
         throw new System.NotImplementedException();
      }

      public IEnumerable<int> UpdateFromExcel(IEnumerable items)
      {
         throw new System.NotImplementedException();
      }
   }
}
