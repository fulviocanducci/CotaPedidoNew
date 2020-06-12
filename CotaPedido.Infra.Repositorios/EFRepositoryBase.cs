using CotaPedido.Dominio.Base;
using CotaPedido.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CotaPedido.Infra.Repositorios
{
    public abstract class EFRepositoryBase<TEntity, TContext> : RepositoryBase<TEntity>, IEFRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        #region Construtores

        public EFRepositoryBase(TContext context)
        {
            this.Context = context;
            this.Query = this.Context.Set<TEntity>().AsQueryable();
        }

        #endregion

        #region Propriedades Protegidas

        protected TContext Context { get; private set; }

        protected IQueryable<TEntity> Query { get; set; }

        #endregion

        #region Métodos Públicos

        public override int Insert(TEntity entity)
        {
            this.Context.Set<TEntity>().Add(entity);
            return this.Context.SaveChanges();
        }

        public override int Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Unchanged;
                this.Context.Set<TEntity>().Attach(entity);
            }

            entry.State = EntityState.Modified;

            return this.Context.SaveChanges();
        }

        public override int Delete(TEntity entity)
        {
            this.Context.Set<TEntity>().Attach(entity);

            this.Context.Set<TEntity>().Remove(entity);

            return this.Context.SaveChanges();
        }

        public override TEntity Get(int id)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "t");

            var leftExp = Expression.PropertyOrField(parameter, "Id");

            var whereClause = Expression.Equal(leftExp, Expression.Constant(id));

            var where = Expression.Lambda<Func<TEntity, bool>>(whereClause, parameter);

            return this.Context.Set<TEntity>().AsNoTracking().AsQueryable().Where(where).SingleOrDefault();
        }

        public override List<TEntity> GetAll()
        {
            this.Query = this.Context.Set<TEntity>().AsQueryable();

            return this.Query.ToList();
        }

        public override List<TEntity> GetList(Dictionary<string, object> filters)
        {
            this.Query = this.Context.Set<TEntity>().AsQueryable();

            SetWhereClauses(filters);

            return this.Query.ToList();
        }

        public T LoadEntityRepository<T>()
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), this.Context);
            }
            catch
            {
                return default(T);
            }
        }

        public override List<TEntity> GetList(object filter)
        {
            this.Query = this.Context.Set<TEntity>().AsQueryable();

            SetWhereClauses(filter);

            return this.Query.ToList();
        }

        #endregion

        #region Métodos Protegidos

        protected virtual void SetWhereClauses(Dictionary<string, object> filters) { }

        protected virtual void SetWhereClauses(object filter) { }

        #endregion
    }
}
