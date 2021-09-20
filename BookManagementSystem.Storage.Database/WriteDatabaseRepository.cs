using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using BookManagementSystem.Storage.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Storage.Database
{
    public class WriteDatabaseRepository<T, TKey> : IWriteDatabaseRepository<T, TKey> where T : BaseEntity<TKey>
    {
        protected DbSet<T> DbSet;

        public WriteDatabaseRepository(ApplicationDbContext context)
        {
            this.DbSet = context.Set<T>();
        }
        public virtual IQueryable<T> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual async Task<T> GetById(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<int> Count()
        {
            return await DbSet.CountAsync();

        }
        public virtual async Task<bool> Add(T entity)
        {
            await DbSet.AddAsync(entity);
            return true;
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task<bool> Delete(TKey id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity == null)
                return false;
            DbSet.Remove(entity);
            return true;
        }

      
    }
}