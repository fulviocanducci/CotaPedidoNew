using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotaPedido.Infra.Repositorios
{
    public class EFRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext, new()
    {
        protected TContext context { get; set; }
        protected DbSet<TEntity> dbSet { get; set; }
        public EFRepository(TContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public bool Inserir(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            context.Entry(entity).State = EntityState.Added;
            dbSet.Add(entity);

            return context.SaveChanges() > 0;
        }

        public bool Inserir(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (context.Entry(entity).State == EntityState.Detached)
                    dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Added;
                dbSet.Add(entity);
            }

            return context.SaveChanges() > 0;
        }

        public bool Editar(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            context.Entry(entity).State = EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public bool Editar(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (context.Entry(entity).State == EntityState.Detached)
                    dbSet.Attach(entity);

                context.Entry(entity).State = EntityState.Modified;    
            }

            return context.SaveChanges() > 0;
        }

        public bool Excluir(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            context.Entry(entity).State = EntityState.Deleted;

            return context.SaveChanges() > 0;
        }

        public List<TEntity> Listar(Func<TEntity, bool> where)
        {
            return dbSet.Where(where).ToList();
        }

        public List<TEntity> Listar()
        {
            return dbSet.ToList();
        }

        public TEntity Obter(int id)
        {
            return dbSet.Find(id);
        }
    }
}
