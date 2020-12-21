using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MultipleEmailsSendler.Service.Interfaces
{

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        TEntity FindById(int id);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);

        void Remove(TEntity item);
        void Update(TEntity item);
    }
}
