using System;
using System.Collections.Generic;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioVendedor : EFRepositoryBase<Vendedor, COTAPEDIDOContext>, IRepositorioVendedor
    {
        public RepositorioVendedor()
            : base(new COTAPEDIDOContext())
        {}

      public Vendedor Get(string email, string senha)
      {
         throw new NotImplementedException();
      }

        public bool GetEmailExists(string email)
        {
            throw new NotImplementedException();
        }

        public int Insert(string email, string senha, string nomeFantasia, string razaoSocial, int idCidade, int idCidadeCobranca, DateTime dataCadastro, bool status = true)
        {
            throw new NotImplementedException();
        }
    }
}
