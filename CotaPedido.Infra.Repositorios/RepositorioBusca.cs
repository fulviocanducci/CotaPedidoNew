using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System.Collections.Generic;
using System;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
   public class RepositorioBusca : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioBusca
    {
        public RepositorioBusca()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(Busca entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Busca entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Busca entity)
        {
            throw new NotImplementedException();
        }

      Busca IRepository<Busca>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<Busca> IRepository<Busca>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<Busca> IRepository<Busca>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<Busca> IRepository<Busca>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
