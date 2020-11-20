using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MultipleEmailsSendler.Service.Interfaces;

namespace MultipleEmailsSendler.Service
{
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EfGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            
            

  
            return _dbSet.AsNoTracking().AsEnumerable().Where(predicate).ToList();
        }
        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
    }
}