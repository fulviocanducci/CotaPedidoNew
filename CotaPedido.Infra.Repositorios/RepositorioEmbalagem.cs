using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;
using System.Collections.Generic;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioEmbalagem : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioEmbalagem
    {
        public RepositorioEmbalagem()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(Embalagem entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Embalagem entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Embalagem entity)
        {
            throw new NotImplementedException();
        }

      Embalagem IRepository<Embalagem>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<Embalagem> IRepository<Embalagem>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<Embalagem> IRepository<Embalagem>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<Embalagem> IRepository<Embalagem>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
