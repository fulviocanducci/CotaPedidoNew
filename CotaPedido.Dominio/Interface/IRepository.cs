using System.Collections.Generic;

namespace CotaPedido.Dominio.Interface
{
    public interface IRepository
    {
        #region Assinatura de Métodos

        int Insert(object entity);

        int Update(object entity);

        int Delete(object entity);

        object Get(object id);

        List<object> GetAll();

        List<object> GetList(Dictionary<string, object> filters);

        List<object> GetList(object filter);

        #endregion
    }

    public interface IRepository<TEntity> where TEntity : class
    {
        #region Assinatura de Métodos

        int Insert(TEntity entity);

        int Update(TEntity entity);

        int Delete(TEntity entity);

        TEntity Get(int id);

        List<TEntity> GetAll();

        List<TEntity> GetList(Dictionary<string, object> filters);

        List<TEntity> GetList(object filter);

        #endregion
    }
}
