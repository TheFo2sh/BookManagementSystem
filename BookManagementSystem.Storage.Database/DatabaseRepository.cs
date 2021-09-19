using System;
using System.Collections.Generic;
using System.Linq;
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

        public DatabaseRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = context.Set<T>();
        }

       
        public virtual IQueryable<T> All()
        {
            return  dbSet.AsQueryable();
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

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual async Task<bool> Delete(TKey id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
                return false;
            dbSet.Remove(entity);
            return true;
        }

    

        public async Task<int> Count()
        {
            return await dbSet.CountAsync();

        }
    }
}