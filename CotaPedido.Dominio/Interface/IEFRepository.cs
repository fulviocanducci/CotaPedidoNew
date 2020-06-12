using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotaPedido.Dominio.Interface
{
    public interface IEFRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Assinatura de Métodos

        T LoadEntityRepository<T>();

        #endregion
    }
}
