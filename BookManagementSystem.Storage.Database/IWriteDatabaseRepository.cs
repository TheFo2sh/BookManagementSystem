using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Storage.Database
{
    public interface IWriteDatabaseRepository<T, TKey>  where T : class
    {
        IQueryable<T> All();
        Task<T> GetById(TKey id);
        Task<int> Count();
        Task<bool> Add(T entity);
        void Update(T entity);
        Task<bool> Delete(TKey id);
    }
}