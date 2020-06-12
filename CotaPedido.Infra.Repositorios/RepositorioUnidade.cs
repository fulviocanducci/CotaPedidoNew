using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;
using System.Collections.Generic;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioUnidade : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioUnidade
    {
        public RepositorioUnidade()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(Unidade entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Unidade entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Unidade entity)
        {
            throw new NotImplementedException();
        }

      Unidade IRepository<Unidade>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<Unidade> IRepository<Unidade>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<Unidade> IRepository<Unidade>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<Unidade> IRepository<Unidade>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
