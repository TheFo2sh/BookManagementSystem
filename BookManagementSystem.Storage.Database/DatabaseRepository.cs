using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Storage.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;

namespace BookManagementSystem.Storage.Database
{
    public class ReadDatabaseRepository<T, TKey> : IReadDatabaseRepository<T,TKey> where T : BaseEntity<TKey>
    {
        protected DbSet<T> DbSet;

        public ReadDatabaseRepository(ApplicationDbContext context)
        {
            this.DbSet = context.Set<T>();
        }

       
        public virtual IQueryable<T> All()
        {
            return  DbSet.AsQueryable();
        }

        public virtual async Task<T> GetById(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<int> Count()
        {
            return await DbSet.CountAsync();

        }
    }
}