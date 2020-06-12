using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using CotaPedido.EntityFramework.Models;

namespace CotaPedido.Infra.Repositorios
{
    public class RepositorioSubGrupo : EFRepositoryBase<SubGrupo, COTAPEDIDOContext>, IRepositorioSubGrupo
    {
        public RepositorioSubGrupo()
            : base(new COTAPEDIDOContext())
        {}

        public SubGrupo Get(string subGrupo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SubGrupo> GetListOfGrupo(int idGrupo)
      {
         throw new NotImplementedException();
      }
   }
}
