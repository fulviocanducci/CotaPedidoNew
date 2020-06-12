using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;
using System.Collections.Generic;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioMensalidade : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioMensalidade
    {
        public RepositorioMensalidade()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(Mensalidade entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Mensalidade entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Mensalidade entity)
        {
            throw new NotImplementedException();
        }

      Mensalidade IRepository<Mensalidade>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<Mensalidade> IRepository<Mensalidade>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<Mensalidade> IRepository<Mensalidade>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<Mensalidade> IRepository<Mensalidade>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
