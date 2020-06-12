using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;
using System.Collections.Generic;
using CotaPedido.Dominio.Interface;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioAssinatura : EFRepositoryBase<ItemPedido, COTAPEDIDOContext>, IRepositorioAssinatura
    {
        public RepositorioAssinatura()
            : base(new COTAPEDIDOContext())
        {}

        public int Insert(Assinatura entity)
        {
            throw new NotImplementedException();
        }

        public int Update(Assinatura entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Assinatura entity)
        {
            throw new NotImplementedException();
        }

        

      public IEnumerable<AssinaturaViewModel> GetVendedorId(int id)
      {
         throw new NotImplementedException();
      }

      public bool GetVendedorIdIsAssinante(int id)
      {
         throw new NotImplementedException();
      }

      public int AtualizaStatusPagamento(Assinatura entity)
      {
         throw new NotImplementedException();
      }

      Assinatura IRepository<Assinatura>.Get(int id)
      {
         throw new NotImplementedException();
      }

      List<Assinatura> IRepository<Assinatura>.GetAll()
      {
         throw new NotImplementedException();
      }

      List<Assinatura> IRepository<Assinatura>.GetList(Dictionary<string, object> filters)
      {
         throw new NotImplementedException();
      }

      List<Assinatura> IRepository<Assinatura>.GetList(object filter)
      {
         throw new NotImplementedException();
      }
   }
}
