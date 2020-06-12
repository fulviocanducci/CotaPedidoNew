
namespace CotaPedido.Dominio.Interface
{
    public interface ISqlRepository<TEntity> : IRepository<TEntity>
       where TEntity : class
    {
    }
}
