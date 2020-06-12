using CotaPedido.Dominio.Interface;
using System.Collections.Generic;

namespace CotaPedido.Dominio.Base
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Métodos Públicos

        public abstract int Insert(TEntity entity);

        public abstract int Update(TEntity entity);

        public abstract int Delete(TEntity entity);

        public abstract TEntity Get(int id);

        public abstract List<TEntity> GetAll();

        public abstract List<TEntity> GetList(Dictionary<string, object> filters);

        public abstract List<TEntity> GetList(object filter);

        #endregion
    }
}
