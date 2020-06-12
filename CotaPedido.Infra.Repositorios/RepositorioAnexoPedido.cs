using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;
using System.Collections.Generic;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioAnexoPedido : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioAnexoPedido
    {
        public RepositorioAnexoPedido()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(AnexoPedido entity)
        {
            throw new NotImplementedException();
        }

        public int Update(AnexoPedido entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(AnexoPedido entity)
        {
            throw new NotImplementedException();
        }

        public new AnexoPedido Get(int id)
        {
            throw new NotImplementedException();
        }

        public new List<AnexoPedido> GetAll()
        {
            throw new NotImplementedException();
        }

      List<AnexoPedido> IRepository<AnexoPedido>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<AnexoPedido> IRepository<AnexoPedido>.GetList(object filter)
      {
         throw new NotImplementedException();
      }


      //public List<AnexoPedido> GetList(Dictionary<string, object> filters)
      //{
      //    throw new NotImplementedException();
      //}

      //public List<AnexoPedido> GetList(object filter)
      //{
      //    throw new NotImplementedException();
      //}
   }
}
