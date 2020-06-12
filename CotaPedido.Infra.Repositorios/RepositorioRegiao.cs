using CotaPedido.Dominio.Interface;
using CotaPedido.Entidades;
using CotaPedido.EntityFramework.Models;
using CotaPedido.Infra.IRepositorios;
using System;
using System.Collections.Generic;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioRegiao : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioRegiao
    {
        public RepositorioRegiao()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(Regiao entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Regiao entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Regiao entity)
        {
            throw new NotImplementedException();
        }

      Regiao IRepository<Regiao>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<Regiao> IRepository<Regiao>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<Regiao> IRepository<Regiao>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<Regiao> IRepository<Regiao>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
