using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;
using System;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioComprador : EFRepositoryBase<Comprador, COTAPEDIDOContext>, IRepositorioComprador
    {
        public RepositorioComprador()
            : base(new COTAPEDIDOContext())
        {}

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
