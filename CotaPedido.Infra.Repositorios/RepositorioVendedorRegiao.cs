using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;
using System.Collections.Generic;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioVendedorRegiao : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioVendedorRegiao
    {
        public RepositorioVendedorRegiao()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(VendedorRegiao entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(VendedorRegiao entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(VendedorRegiao entity)
        {
            throw new NotImplementedException();
        }

      VendedorRegiao IRepository<VendedorRegiao>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<VendedorRegiao> IRepository<VendedorRegiao>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<VendedorRegiao> IRepository<VendedorRegiao>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<VendedorRegiao> IRepository<VendedorRegiao>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
