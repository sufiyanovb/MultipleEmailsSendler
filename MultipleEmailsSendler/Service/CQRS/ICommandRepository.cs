using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MultipleEmailsSendler.Service.Interfaces
{
    public interface ICommandRepository<TEntity> where TEntity : class
    {
        TEntity FindById(int id);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
     
    }
}
