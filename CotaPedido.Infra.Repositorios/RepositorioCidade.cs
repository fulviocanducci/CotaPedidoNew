using System;
using System.Collections.Generic;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System.Collections;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioCidade : EFRepositoryBase<Cidade, COTAPEDIDOContext>, IRepositorioCidade
    {
        public RepositorioCidade()
            : base(new COTAPEDIDOContext())
        {}

      public IEnumerable GetAllWithUf()
      {
         throw new NotImplementedException();
      }
   }
}
