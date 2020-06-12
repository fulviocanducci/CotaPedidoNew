using CotaPedido.Dominio.Base;
using CotaPedido.Dominio.Interface;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace CotaPedido.Infra.Repositorios.SQLServer
{
    public abstract class SqlRepositoryBase<TEntity> : RepositoryBase<TEntity>, ISqlRepository<TEntity>
        where TEntity : class
    {
        #region Atributos Protegidos

        protected string _connectionString;
        protected IDbConnection _connection;
        protected readonly dynamic _parameters;

        #endregion

        #region Propriedades Protegidas

        protected string Query { get; set; }

        #endregion

        #region Construtores

        public SqlRepositoryBase()
        {
            _parameters = new ExpandoObject();
        }

        #endregion

        #region Métodos Protegidos

        protected virtual void SetWhereClauses(Dictionary<string, object> filters) { }

        protected virtual void SetWhereClauses(object filter) { }

        protected virtual void InitializeQuery() { }

        #endregion
    }
}
