using CotaPedido.Entidades;
using CotaPedido.Infra.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public class RepositorioEmbalagem : SqlRepositoryBase<AnexoItem>, IRepositorioAnexoItem
    {
        #region Construtores

        public RepositorioEmbalagem()
            : base()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["cotapedido"].ConnectionString;

            InitializeQuery();
        }

        #endregion

        #region Métodos Publicos

        public override int Insert(AnexoItem entity)
        {
            throw new NotImplementedException();
        }

        public override int Update(AnexoItem entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(AnexoItem entity)
        {
            throw new NotImplementedException();
        }

        public override AnexoItem Get(int id)
        {
            throw new NotImplementedException();
        }

        public override List<AnexoItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public override List<AnexoItem> GetList(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }

        public override List<AnexoItem> GetList(object filter)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
