using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementSystem.Storage.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookManagementSystem.Storage.Database
{
    public class DatabaseRepository<T, TKey> : IDatabaseRepository<T,TKey> where T : BaseEntity<TKey>
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> dbSet;
        protected readonly ILogger _logger;

        public DatabaseRepository(
            ApplicationDbContext context,
            ILogger logger
        )
        {
            _context = context;
            _logger = logger;
            this.dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> GetById(TKey id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(TKey id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
                return false;
            dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<bool> Upsert(T entity)
        {
            var result = await dbSet.FindAsync(entity.Id);
            if (result == null)
                return await Add(entity);
            
            entity.CopyTo(result);
            
            return true;
        }
    }
}